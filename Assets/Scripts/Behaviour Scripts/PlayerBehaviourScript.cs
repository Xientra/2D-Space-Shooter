using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviourScript : MonoBehaviour {

    public GameObject CreateOnDeath;

    public GameObject TurretRotationAnchorGo;
    private GameObject TurretGameObject;

    public GameObject ObjectHolderGo;

    private GameObject[] Bullets;

    private Vector3 ProjectileSpawnPoint;

    /*----LookVars----*/
    private bool lookForwardWhenMoving = false;
    private Vector3 lastFramePos;

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

    public GameObject firstWeapon;
    public GameObject secondWeapon;


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

        lastFramePos = transform.position;


        StartCoroutine(DoStuffAfterOneFrame());
    }

    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;

        if (firstWeapon != null && secondWeapon != null) {
            firstWeapon = GameControllerScript.PlayerFirstWeapon;
            secondWeapon = GameControllerScript.PlayerSecondWeapon;

            ChangeTurret(firstWeapon.GetComponent<WeaponBehaviourScript>().WeaponType);
        }
        else
            Debug.LogError("When trying to assing the player weapons at least one of GameControllerWeapons was null. (this probably means, that the playership tried to assing them befor the gameController had them.)");
    }

    void Update() {

        if (GameControllerScript.GameIsPaused != true) {

            LookForward();
            RotateTurret();

            if (Input.GetButton("Fire1")) {
                FireWeapon(firstWeapon);
            }
            //if (Input.GetButton("Fire2")) {
            //    FireWeapon(secondWeapon);
            //}
            if (Input.GetButtonDown("Fire2")) {
                SwitchWeapon();
            }
        }

        if (regenerates == true) {
            if (currendHealth < MaxHealth) {
                currendHealth += regenerationSpeed;
            }
            else regenerates = false;
        }

        ProjectileSpawnPoint = TurretRotationAnchorGo.transform.position;
        //ProjectileSpawnPoint = TurretRotationAnchorGo.transform.position + (TurretRotationAnchorGo.transform.up * (transform.localScale.y / 5f));
        if (TurretGameObject != null) {
            foreach (Transform tChild in TurretGameObject.transform) {
                if (tChild.gameObject.name == "ProjectileSpawnPointGo") ProjectileSpawnPoint = tChild.position;
            }
            if (ProjectileSpawnPoint == TurretRotationAnchorGo.transform.position) Debug.Log("The ProjectileSpawnPoint is the same as the TurretAnchor. This means, that the Point has not been moved yet or it does not exist at all");
        }
        

        CheckPlayerDeath();
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
                LaserBulletBehaviourScript.damageMultiplyer = 1.2f;
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
                LaserBulletBehaviourScript.damageMultiplyer = 1f;
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

    void RotateTurret() {
        if (GameControllerScript.UsingGamepad == false) {
            TurretRotationAnchorGo.transform.up = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - transform.position;
        }
        else {
            Vector2 ShootDirection = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
            if (Input.GetAxis("HorizontalRight") != 0 || Input.GetAxis("VerticalRight") != 0)
                TurretRotationAnchorGo.transform.right = ShootDirection.normalized;
        }
    }

    void CheckPlayerDeath() {
        if (currendHealth > MaxHealth) currendHealth = MaxHealth;


        if (currendHealth <= 0) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().StartGameOver(2.3f);

            Instantiate(CreateOnDeath, transform.position, transform.rotation);

            Destroy(this.gameObject);
        }
    }

    void adjustScale(float xscale, float yscale) {
        transform.localScale = new Vector3(xscale, yscale, transform.localScale.z);
        this.gameObject.GetComponent<TrailRenderer>().startWidth = yscale / 10;
    }

    void FireWeapon(GameObject _weapon) {
        if (cooldownTimeStamp <= Time.time) {
            cooldown = _weapon.GetComponent<WeaponBehaviourScript>().cooldown * fireRateMultiplyer;
            _weapon.GetComponent<WeaponBehaviourScript>().Fire(ProjectileSpawnPoint, TurretRotationAnchorGo);
            cooldownTimeStamp = Time.time + cooldown;
        }
        {
            /*
            switch (currentWeapon) {
                case (Weapons.Standart_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Standart_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * 0.1f), TurretRotationAnchorGo.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * -0.1f), TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Standart_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.4f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * 0.2f), TurretRotationAnchorGo.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * -0.2f), TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Helix_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.5f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_1)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Helix_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.7f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_2)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Helix_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.9f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_3)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Spread):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.4f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, -10));
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, 10));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Homing_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float BulletRng = 1f;
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_1)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Homing_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.2f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float BulletRng = 6f;
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_2)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.Homing_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.1f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float BulletRng = 12f;
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_3)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.LaserSword_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.5f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            float fieldSize = 1.25f;
                            Vector3 RndFieldPos = transform.position + (TurretRotationAnchorGo.transform.right * Random.Range(-fieldSize, fieldSize)) + (TurretRotationAnchorGo.transform.up * Random.Range(-fieldSize, -fieldSize / 2));//new Vector3(Random.Range(-fieldSize, fieldSize), ;
                            Quaternion LookToMouse = Quaternion.LookRotation(Vector3.forward, (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - RndFieldPos));
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.LaserSword_lvl_1)], RndFieldPos, LookToMouse);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.WaveEmitter_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Wave)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.RocketLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Rocket)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.GrenadeLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Grenade)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ShrapnelLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_1)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ShrapnelLauncher_lvl_2):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 1.2f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_2)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.ShrapnelLauncher_lvl_3):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 1.4f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_3)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
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
                                    Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.SimpleLaser)], transform.position, transform.rotation, this.transform);
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
                    fireAnyLaserGun(LaserBulletBehaviourScript.BulletTypes.SplitLaser);
                    break;
                default:
                    Debug.LogError("The Weapon Type -" + currentWeapon.ToString() + "- has no values assinged!");
                    break;
            }
            */
        }
    }

    private void fireChainGun(float offsetSpeed, float maxOffset, float BulletRng) {
        Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.ChainGunBullet)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * chainGunOffset), TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
        if (chainGunOffsetUp) chainGunOffset += offsetSpeed;
        else chainGunOffset -= offsetSpeed;
        if (chainGunOffset >= maxOffset) chainGunOffsetUp = false;
        if (chainGunOffset <= -maxOffset) chainGunOffsetUp = true;
    }

    private void fireAnyLaserGun(LaserBulletBehaviourScript.BulletTypes LaserToFire) {
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
                        cooldownTimeStamp = Time.time + cooldown; //Applies cooldown when firered
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
    
    void SwitchWeapon() {
        GameObject tempWep = firstWeapon;
        firstWeapon = secondWeapon;
        secondWeapon = tempWep;
        ChangeTurret(firstWeapon.GetComponent<WeaponBehaviourScript>().WeaponType);
    }
    
    void ChangeTurret(WeaponBehaviourScript.WeaponTypes _weaponType) {
        //destroys existing turret
        if (TurretRotationAnchorGo.GetComponentsInChildren<Transform>().Length > 0)
            for (int i = 1; i <= TurretRotationAnchorGo.GetComponentsInChildren<Transform>().Length - 1; i++) { /*-1 da der erste transform des arrays nicht zerstört werden soll*/
                Destroy(TurretRotationAnchorGo.GetComponentsInChildren<Transform>()[i].gameObject);
            }

        //spawns new turret
        if (firstWeapon.GetComponent<WeaponBehaviourScript>().TurretGameObject != null) {
            TurretGameObject = Instantiate(firstWeapon.GetComponent<WeaponBehaviourScript>().TurretGameObject, TurretRotationAnchorGo.transform);
        }
        else {
            TurretGameObject = Instantiate(ObjectHolder._Turrets[ObjectHolder.GetWeaponTurretIndex(WeaponBehaviourScript.WeaponTypes.Standart_lvl_1)], TurretRotationAnchorGo.transform);
            //TurretGameObject = Instantiate(ObjectHolder._Turrets[ObjectHolder.GetWeaponTurretIndex(_weaponType)], TurretRotationAnchorGo.transform);
        }
    }

    public float GetPercentUnitCooldown() {
        float PercentUnitlCooldown = -(((cooldownTimeStamp - Time.time) / cooldown) * 100) + 100;
        PercentUnitlCooldown = Mathf.Clamp(PercentUnitlCooldown, 0, 100);

        return PercentUnitlCooldown;
        /*
        cooldown = 0.005f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            cooldownTimeStamp = Time.time + cooldown;
        }
        */
    }

    void LookForward() {
        if (lookForwardWhenMoving == true) {
            Vector3 lookDirection = transform.position - lastFramePos;
            Vector3.Normalize(lookDirection);

            transform.up = -lookDirection;

            lastFramePos = transform.position;
        }
    }
}
