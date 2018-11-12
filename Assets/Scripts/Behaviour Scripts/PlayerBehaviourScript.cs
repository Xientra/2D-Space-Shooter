using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour {

    public GameObject TurretGameObject;
    public GameObject GameControllerGo;
    public GameObject ObjectHolderGo;

    private GameObject[] Bullets;

    private Vector3 ProjectileSpawnPoint;

    public GameObject firstWeapon;
    public GameObject secondWeapon;

    /*Movement Vars*/
    [SerializeField]
    private float acceleration = 1f;
    [SerializeField]
    private float speedlimit = 10f;
    [SerializeField]
    private float stopspeed = 0.9f;    
    [SerializeField]
    private float xspeed = 0;
    [SerializeField]
    private float yspeed = 0;

    public float MaxHealth = 100f;
    public float currendHealth = 100f;

    //Ship Stats
    public enum Ships { Standart, Heavy, Fast }
    public Ships currendShip = Ships.Standart;


    /*----------Weapon Stuff----------*/
    private float cooldown = 0.2f;
    private float cooldownTimeStamp;
    //laser firering vars
    private float loadTime = 0.2f;
    private float loadTimeStamp;
    private bool isReady = false;
    private bool pressedButtonDown = false;

    private float chainGunOffset = 0;
    private bool chainGunOffsetUp = true;

    /*----------Stuff for PickUps----------*/
    private static float fireRateMultiplyer = 1;
    public static bool regenerates = false;
    private static float regenerationSpeed = 0.1f; //Not framerate indipendent


    /*---------------------------------------------End-Of-Variables---------------------------------------------------------------------------*/
    void Start() {
        Bullets = ObjectHolder._Bullets;

        StartCoroutine(DoStuffAfterOneFrame());

    }

    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;
        firstWeapon = MainMenuControllerScript.firstWeaponGO; //ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Standart_lvl_1)];
        secondWeapon = MainMenuControllerScript.secondWeaponGO; //ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1)];
        ChangeTurret(firstWeapon.GetComponent<WeaponBehaviourScript>().WeaponType);
    }

    void Update() {

        if (GameControllerScript.UsingGamepad == false) {
            TurretGameObject.transform.up = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - transform.position;
        }
        else {
            Vector2 ShootDirection = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
            if (Input.GetAxis("HorizontalRight") != 0 || Input.GetAxis("VerticalRight") != 0)
                TurretGameObject.transform.right = ShootDirection.normalized;
        }
        
        if (Input.GetButton("Fire1")) {
            FireWeapon(firstWeapon);
        }
        if (Input.GetButton("Fire2")) {
            FireWeapon(secondWeapon);
        }

        if (regenerates == true) {
            if (currendHealth < MaxHealth) {
                currendHealth += regenerationSpeed;
            }
            else regenerates = false;
        }

        ProjectileSpawnPoint = TurretGameObject.transform.position;
        ProjectileSpawnPoint = TurretGameObject.transform.position + (TurretGameObject.transform.up * (transform.localScale.y / 5f));

        if (currendHealth <= 0) Destroy(this.gameObject);
        if (currendHealth > MaxHealth) currendHealth = MaxHealth;
    }

    void FixedUpdate() {
        coolMovement();
        //Movement();
        //exactMovement2();
        //exactMovement();
        //coinMovement();
        //OldMovement();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 8) {
            xspeed = 0;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PickUp")) {
            if (collision.GetComponent<PickUpBehaviourScript>().thisPickUpType == PickUpBehaviourScript.PickUpTypes.Credit) {
                GameControllerScript.currendCredits += collision.GetComponent<PickUpBehaviourScript>().CreditValue;
                Destroy(collision.gameObject);
            }
            else if(collision.GetComponent<PickUpBehaviourScript>().thisPickUpType == PickUpBehaviourScript.PickUpTypes.HealthUp) {
                currendHealth += MaxHealth * 0.2f;
            }
            else if (collision.GetComponent<PickUpBehaviourScript>().thisPickUpType == PickUpBehaviourScript.PickUpTypes.Regeneration) {
                regenerates = true;
            }
            else
                StartCoroutine(ActivatePowerUpforTime((int)collision.gameObject.GetComponent<PickUpBehaviourScript>().thisPickUpType, 5f));
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ActivatePowerUpforTime(int PowerUpNr ,float TimeToWait) {
        Debug.Log((PickUpBehaviourScript.PickUpTypes)PowerUpNr + " started."); //Update some UI or stuff pls

        switch ((PickUpBehaviourScript.PickUpTypes)PowerUpNr) {
            case (PickUpBehaviourScript.PickUpTypes.FireRateUp):
                fireRateMultiplyer = 0.6f;
                break;
            case (PickUpBehaviourScript.PickUpTypes.DamageUp):
                LaserBulletData.damageMultiplyer = 1.2f;
                break;
            case (PickUpBehaviourScript.PickUpTypes.Invincibility):
                EnemyBehaviourScript.noCollisionDamage = true;
                break;
            case (PickUpBehaviourScript.PickUpTypes.SloMo):
                Time.timeScale = 0.8f;
                break;
            default:
                Debug.LogError("The PickUp -" + (PickUpBehaviourScript.PickUpTypes)PowerUpNr + "- has no effect assinged!");
                break;
        }

        yield return new WaitForSeconds(TimeToWait);

        switch ((PickUpBehaviourScript.PickUpTypes)PowerUpNr) {
            case (PickUpBehaviourScript.PickUpTypes.FireRateUp):
                fireRateMultiplyer = 1f;
                break;
            case (PickUpBehaviourScript.PickUpTypes.DamageUp):
                LaserBulletData.damageMultiplyer = 1f;
                break;
            case (PickUpBehaviourScript.PickUpTypes.Invincibility):
                EnemyBehaviourScript.noCollisionDamage = false;
                break;
            case (PickUpBehaviourScript.PickUpTypes.SloMo):
                Time.timeScale = 1f;
                break;
            default:
                Debug.LogError("The PickUp -" + (PickUpBehaviourScript.PickUpTypes)PowerUpNr + "- has no effect assinged!");
                break;
        }

        Debug.Log((PickUpBehaviourScript.PickUpTypes)PowerUpNr + " stoped"); //Update some UI or stuff pls
    }

    void coolMovement() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            float xacc = Input.GetAxis("Horizontal") * acceleration;
            float yacc = Input.GetAxis("Vertical") * acceleration;
            xspeed += xacc;
            yspeed += yacc;

            /* DS2...
            if (xspeed > speedlimit) { xspeed = speedlimit; }
            if (xspeed < -speedlimit) { xspeed = -speedlimit; }
            if (yspeed > speedlimit) { yspeed = speedlimit; }
            if (yspeed < -speedlimit) { yspeed = -speedlimit; }
            */
        }
        else { }

        //Stopping
        xspeed *= stopspeed;
        yspeed *= stopspeed;
        if (xspeed < 0.001f && xspeed > 0 || xspeed > -0.001f && xspeed < 0) xspeed = 0;
        if (yspeed < 0.001f && yspeed > 0 || yspeed > -0.001f && yspeed < 0) yspeed = 0;

        //Actual Moving
        transform.Translate(xspeed * Time.deltaTime, yspeed * Time.deltaTime, 0);
    }

    void adjustScale(float xscale, float yscale) {
        transform.localScale = new Vector3(xscale, yscale, transform.localScale.z);
        this.gameObject.GetComponent<TrailRenderer>().startWidth = yscale / 10;
    }

    void FireWeapon(GameObject _weapon) {
        if (cooldownTimeStamp <= Time.time) {
            cooldown = _weapon.GetComponent<WeaponBehaviourScript>().cooldown * fireRateMultiplyer;
            _weapon.GetComponent<WeaponBehaviourScript>().Fire(ProjectileSpawnPoint, TurretGameObject);
            cooldownTimeStamp = Time.time + cooldown;
        }
        {
            /*
            switch (currentWeapon) {
                case (Weapons.Standart_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Standart_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * 0.1f), TurretGameObject.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * -0.1f), TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Standart_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.4f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * 0.2f), TurretGameObject.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * -0.2f), TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Helix_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.5f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HelixBullet_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Helix_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.7f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HelixBullet_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Helix_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.9f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HelixBullet_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Spread):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.4f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -10));
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 10));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Homing_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float BulletRng = 1f;
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HomingBullet_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Homing_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.2f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float BulletRng = 6f;
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HomingBullet_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Homing_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.1f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float BulletRng = 12f;
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HomingBullet_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.LaserSword_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.5f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float fieldSize = 1.25f;
                            Vector3 RndFieldPos = transform.position + (TurretGameObject.transform.right * Random.Range(-fieldSize, fieldSize)) + (TurretGameObject.transform.up * Random.Range(-fieldSize, -fieldSize / 2));//new Vector3(Random.Range(-fieldSize, fieldSize), ;
                            Quaternion LookToMouse = Quaternion.LookRotation(Vector3.forward, (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - RndFieldPos));
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.LaserSword_lvl_1)], RndFieldPos, LookToMouse);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.WaveEmitter_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Wave)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.RocketLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Rocket)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.GrenadeLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Grenade)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ShrapnelLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Shrapnel_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ShrapnelLauncher_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 1.2f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Shrapnel_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ShrapnelLauncher_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 1.4f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Shrapnel_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ChainGun_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.01f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            fireChainGun(0.04f, 0.1f, 6f);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ChainGun_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.01f * fireRateMultiplyer * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            fireChainGun(0.04f, 0.1f, 3f);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ChainGun_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.005f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            fireChainGun(0.04f, 0.1f, 1f);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.LaserGun):
                    cooldown = 2f * fireRateMultiplyer;
                    loadTime = 1f;
                    if (cooldownTimeStamp <= Time.time) {
                        if (Input.GetButtonDown("Fire1")) {
                            pressedButtonDown = true;
                            loadTimeStamp = Time.time + loadTime;
                            isReady = true;
                        }
                        if (loadTimeStamp >= Time.time) {
                            if (isReady == true) {
                                Debug.Log("Loading");
                            }
                        }
                        if (loadTimeStamp <= Time.time) {
                            if (isReady == true) {
                                Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.LaserLoaded)], transform.position, transform.rotation, transform);
                                isReady = false;
                            }
                        }
                        if (pressedButtonDown == true) {
                            if (Input.GetButtonUp("Fire1")) {
                                if (loadTimeStamp <= Time.time) {
                                    Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.SimpleLaser)], transform.position, transform.rotation, this.transform);
                                    loadTimeStamp = Time.time + loadTime;
                                    cooldownTimeStamp = Time.time + cooldown; //Apply cooldown when firered
                                }
                                else {
                                    loadTimeStamp = Time.time + loadTime;
                                    //no cooldown wehen reset
                                    Debug.Log("reset");
                                }
                                isReady = false;
                                pressedButtonDown = false;
                            }
                        }
                    }
                    break;
                case (Weapons.SplitLaserGun):
                    cooldown = 2f * fireRateMultiplyer;
                    loadTime = 1f;
                    fireAnyLaserGun(LaserBulletData.BulletTypes.SplitLaser);
                    break;
                default:
                    Debug.LogError("The Weapon Type -" + currentWeapon.ToString() + "- has no values assinged!");
                    break;
            }
            */
        }
    }

    private void fireChainGun(float offsetSpeed, float maxOffset, float BulletRng) {
        Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.ChainGunBullet)], ProjectileSpawnPoint + (TurretGameObject.transform.right * chainGunOffset), TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
        if (chainGunOffsetUp) chainGunOffset += offsetSpeed;
        else chainGunOffset -= offsetSpeed;
        if (chainGunOffset >= maxOffset) chainGunOffsetUp = false;
        if (chainGunOffset <= -maxOffset) chainGunOffsetUp = true;
    }

    private void fireAnyLaserGun(LaserBulletData.BulletTypes LaserToFire) {
        if (cooldownTimeStamp <= Time.time) {
            if (Input.GetButtonDown("Fire1")) {
                pressedButtonDown = true;
                loadTimeStamp = Time.time + loadTime;
                isReady = true;
            }
            if (loadTimeStamp >= Time.time) {
                if (isReady == true) {
                    Debug.Log("Loading");
                }
            }
            if (loadTimeStamp <= Time.time) {
                if (isReady == true) {
                    Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.LaserLoaded)], transform.position, transform.rotation, transform);
                    isReady = false;
                }
            }
            if (pressedButtonDown == true) {
                if (Input.GetButtonUp("Fire1")) {
                    if (loadTimeStamp <= Time.time) {
                        Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserToFire)], transform.position, transform.rotation, this.transform);
                        loadTimeStamp = Time.time + loadTime;
                        cooldownTimeStamp = Time.time + cooldown; //Apply cooldown when firered
                    }
                    else {
                        loadTimeStamp = Time.time + loadTime;
                        //no cooldown wehen reset
                        Debug.Log("reset");
                    }
                    isReady = false;
                    pressedButtonDown = false;
                }
            }
        }
    }
    /*
    public int GetBulletIndex(LaserBulletData.BulletTypes _bulletType) {
        int i = 0;
        foreach (GameObject go in Bullets) {
            if (go != null) {
                if (go.GetComponentInChildren<LaserBulletData>().bulletType == _bulletType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _bulletType);
        return -1;
    }
    */

    /*
    void SwitchWeapon() {
        if (currentWeapon == primaryWeapon)
            currentWeapon = secondaryWeapon;
        else {
            if (currentWeapon == secondaryWeapon)
                currentWeapon = primaryWeapon;
            else Debug.LogError("There is a weapon equipt which should not.");
        }
    }

    void SwitchAllWeapons() {
        if ((int)currentWeapon == WeaponBehaviourScript.WeaponTypes.GetNames(typeof(WeaponBehaviourScript.WeaponTypes)).Length - 1) {
            currentWeapon = (WeaponBehaviourScript.WeaponTypes)0;
        }
        else {
            currentWeapon = (WeaponBehaviourScript.WeaponTypes)(int)currentWeapon + 1;
            //ChangeTurret(currentWeapon);
        }
    }
    */
    
    void ChangeTurret(WeaponBehaviourScript.WeaponTypes _weaponType) {
        Destroy(TurretGameObject.GetComponentsInChildren<Transform>()[1].transform.gameObject);
        Instantiate(ObjectHolder._Turrets[ObjectHolder.GetWeaponTurretIndex(_weaponType)], TurretGameObject.transform);
    }

    public float GetPercentUnitCooldown() {
        float PercentUnitlCooldown = -(((cooldownTimeStamp - Time.time) / cooldown) * 100) + 100;
        PercentUnitlCooldown = Mathf.Clamp(PercentUnitlCooldown, 0, 100);

        //Debug.Log(cooldown);
        //Debug.Log(cooldownTimeStamp);
        return PercentUnitlCooldown;
        /*
        cooldown = 0.005f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            cooldownTimeStamp = Time.time + cooldown;
        }
        */
    }
}
