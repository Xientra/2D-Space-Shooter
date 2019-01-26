using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviourScript : MonoBehaviour {

    /*----------Ship Stats----------*/
    public enum Ships { Standart }
    public Ships currendShip = Ships.Standart;
    public float MaxHealth = 100f;
    public float currendHealth = 100f;

    [Space(5)]

    /*----LookVars----*/
    public GameObject ShipGFX; //this is the Go that will be rotated
    public bool lookForwardWhenMoving = true;
    private Vector3 lastFramePos;

    [Space(5)]

    /*----The GameObject for the Turret----*/
    public GameObject TurretRotationAnchorGo; //the empty game object of the player ship which is positioned to where the turret should rotate around 
    private GameObject TurretGameObject; //the GameObject of the Turret itself; it's is own prefab 
    private Vector3 ProjectileSpawnPoint; //will be set to the position of a child of the turrets prefabs; it is used once in WeaponBehaviourScrip.Fire

    [Space(5)]

    public GameObject firstWeapon;
    public GameObject secondWeapon;

    [Space(5)]

    /*Movement Vars*/
    [SerializeField]
    private float acceleration = 1f;
    //[SerializeField]
    //private float speedlimit = 10f;
    [SerializeField]
    private float stopspeed = 0.9f;

    private float xspeed = 0;
    private float yspeed = 0;

    public GameObject CreateOnDeath;


    /*----------Weapon Stuff----------*/
    private float cooldown = 0.2f;
    private float cooldownTimeStamp;


    /*----------Stuff for PickUps----------*/
    private static float fireRateMultiplyer = 1;
    public static bool regenerates = false;
    private static float regenerationSpeed = 5f;
    private GameObject regenerationVisualEffectGo;



    /*---------------------------------------------End-Of-Variables---------------------------------------------------------------------------*/
    void Start() {
        lastFramePos = transform.position;

        CheckForWeapons();
    }

    void CheckForWeapons() {

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
            SetProjectileSpawnPoint();

            if (Input.GetButton("Fire1")) {
                FireWeapon(firstWeapon);
            }
            if (Input.GetButtonDown("Fire2")) {
                SwitchWeapon();
            }

            
            if (regenerates == true) {
                if (currendHealth < MaxHealth) {
                    currendHealth += regenerationSpeed * Time.deltaTime;
                }
                else {
                    regenerates = false;
                    if (regenerationVisualEffectGo != null)
                        Destroy(regenerationVisualEffectGo);
                }
            }
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

            PickUpBehaviourScript _pickUp = collision.GetComponent<PickUpBehaviourScript>();

            if (_pickUp.thisPickUpType == PickUpBehaviourScript.PickUpTypes.Credit) {
                GameControllerScript.currendCredits += collision.GetComponent<PickUpBehaviourScript>().CreditValue;
                //Destroy(collision.gameObject);
            }
            else if(_pickUp.thisPickUpType == PickUpBehaviourScript.PickUpTypes.HealthUp) {
                currendHealth += MaxHealth * 0.2f;
                GameObject Go = Instantiate(_pickUp.VisualEffect, ShipGFX.transform);
                Destroy(Go, 3f);
            }
            else
                StartCoroutine(ActivatePowerUpforTime((int)_pickUp.thisPickUpType, _pickUp.duration, _pickUp.VisualEffect));


            Destroy(collision.gameObject);
        }
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

    void LookForward() {
        if (lookForwardWhenMoving == true) {
            Vector3 lookDirection = transform.position - lastFramePos;
            if (lookDirection.magnitude > 0.01f) {
                Vector3.Normalize(lookDirection);

                ShipGFX.transform.up = lookDirection;
            }
            lastFramePos = transform.position;
        }
    }

    void RotateTurret() {
        if (GameControllerScript.UsingGamepad == false) {
            TurretRotationAnchorGo.transform.up = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - TurretRotationAnchorGo.transform.position;
        }
        else {
            Vector2 ShootDirection = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
            if (Input.GetAxis("HorizontalRight") != 0 || Input.GetAxis("VerticalRight") != 0)
                TurretRotationAnchorGo.transform.right = ShootDirection.normalized;
        }
    }

    void SetProjectileSpawnPoint() { //Sets the ProjectileSpawnPoint to the pos of the ProjectileSpawnPointGo, a child of the turret prefab
        ProjectileSpawnPoint = TurretRotationAnchorGo.transform.position;
        if (TurretGameObject != null) {
            foreach (Transform tChild in TurretGameObject.transform) {
                if (tChild.gameObject.name == "ProjectileSpawnPointGo") ProjectileSpawnPoint = tChild.position;
            }
            if (ProjectileSpawnPoint == TurretRotationAnchorGo.transform.position) Debug.LogWarning("The ProjectileSpawnPoint is the same as the TurretAnchor. This means, that the Point has not been moved yet or it does not exist at all");
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

    void FireWeapon(GameObject _weapon) {
        if (cooldownTimeStamp <= Time.time) {
            cooldown = _weapon.GetComponent<WeaponBehaviourScript>().cooldown * fireRateMultiplyer;
            _weapon.GetComponent<WeaponBehaviourScript>().Fire(ProjectileSpawnPoint, TurretRotationAnchorGo);

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cursor")) {
                go.GetComponent<Animator>().SetTrigger("FireTrigger");
            }

            cooldownTimeStamp = Time.time + cooldown;
        }
    }

    void SwitchWeapon() {
        GameObject tempWep = firstWeapon;
        firstWeapon = secondWeapon;
        secondWeapon = tempWep;
        ChangeTurret(firstWeapon.GetComponent<WeaponBehaviourScript>().WeaponType);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cursor")) {
            go.GetComponent<Animator>().SetTrigger("SwitchWeaponTrigger");
        }
    }

    void ChangeTurret(WeaponBehaviourScript.WeaponTypes _weaponType) {
        //destroys all existing turret
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
            Debug.LogError("When trying to change the turret of the player ");
        }
    }

    IEnumerator ActivatePowerUpforTime(int PowerUpNr, float TimeToWait, GameObject _VisualEffect) {
        //Debug.Log((PickUpBehaviourScript.PickUpTypes)PowerUpNr + " started."); //Update some UI or stuff pls

        bool createEffect = true;

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
            case (PickUpBehaviourScript.PickUpTypes.Regeneration):
                regenerates = true;
                regenerationVisualEffectGo = Instantiate(_VisualEffect, ShipGFX.transform);

                createEffect = false;
                break;
            default:
                Debug.LogError("The PickUp -" + (PickUpBehaviourScript.PickUpTypes)PowerUpNr + "- has no effect assinged!");
                createEffect = false;
                break;

        }

        GameObject tempGo = null;
        if (createEffect == true && _VisualEffect != null) {
            tempGo = Instantiate(_VisualEffect, ShipGFX.transform);
        }

        

        yield return new WaitForSeconds(TimeToWait);

        createEffect = true;

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
            case (PickUpBehaviourScript.PickUpTypes.Regeneration):
                regenerates = false;
                if (regenerationVisualEffectGo != null)
                    Destroy(regenerationVisualEffectGo);

                createEffect = false;
                break;
            default:
                Debug.LogError("The PickUp -" + (PickUpBehaviourScript.PickUpTypes)PowerUpNr + "- has no effect assinged!");
                createEffect = false;
                break;
        }

        if (createEffect == true && _VisualEffect != null) {
            Destroy(tempGo);
        }

        //Debug.Log((PickUpBehaviourScript.PickUpTypes)PowerUpNr + " stoped"); //Update some UI or stuff pls
    }

    void adjustScale(float xscale, float yscale) {
        transform.localScale = new Vector3(xscale, yscale, transform.localScale.z);
        this.gameObject.GetComponent<TrailRenderer>().startWidth = yscale / 10;
    }

    public float GetPercentUnitCooldown() { //used for the cooldown bar in the UI
        float PercentUnitlCooldown = -(((cooldownTimeStamp - Time.time) / cooldown) * 100) + 100;
        PercentUnitlCooldown = Mathf.Clamp(PercentUnitlCooldown, 0, 100);

        return PercentUnitlCooldown;
    }
}