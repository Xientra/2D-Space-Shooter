using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour {
    //public GameObject ObjectHolderGo;
    public GameObject HealthBarGameObject;

    public GameObject[] EnemyTurrets = new GameObject[0];

    private GameObject EnemyHealthBarBackGround;
    private GameObject EnemyHealthBarFill;
    private Animator animator;

    private GameObject[] EnemyBullets;

    public enum ShootTypes {
        None, SingleBullet, FiveSpread, FourSmallLaserBullets, DropBomb
    }
    public ShootTypes ShootType = ShootTypes.None;

    [SerializeField]
    private bool aimTurrets = true;

    [SerializeField]
    private GameObject WeaponProjectile;


    [Header("Animation / Shooting: ")] /*--------------------Animation / Shooting--------------------*/
    public float AnimationStartDelay = 1f;
    [SerializeField]
    private float LimiterDestructionDelayAfterStart = 1f;
    private bool LimiterDestruction = false;

    public bool destroyAfterAnimation = true;

    public bool lookForwardWhenMoving = false;
    private Vector3 lastFramePos;
    private Quaternion startRotation;


    //the movement/behaviour animation change this value for one frame which will make the enemy shoot in that frame
    [HideInInspector]
    public bool canShoot = false; //Has to be Serializable to be able to be changed by an animation

    [SerializeField]
    private float fireDelay = 0.1f;
    private float fireDelayTimeStamp;



    [Header("Stats: ")] /*--------------------Stats--------------------*/
    public float MaxHealth = 100f;
    public float Health = 100f;
    public float CollisionDamage = 50f;


    /*--------------------Drop / Score--------------------*/
    public float scoreValue = 100;

    public float ValueOfCreditDrop = 1f;
    /*
    public enum CoinDropValues {
        _1, _10, _100, _1000
    }
    public CoinDropValues CoinDropValue = CoinDropValues._1;
    */
    int maxCoinDrop = 3;
    int minCoinDrop = 2;
    public float PowerUpDropChangse = 0f; //0 for nothing (whaaat?)

    //PowerUp Variables
    public static bool noCollisionDamage = false;

    /*---------------------------------------------End-Of-Variables---------------------------------------------------------------------------*/
    void Start() {
        //EnemyBullets = ObjectHolder.instance.EnemyBullets;

        Health = MaxHealth;
        lastFramePos = transform.position;
        startRotation = transform.rotation;

        if (GetComponent<Animator>() != null) {
            animator = GetComponent<Animator>();
        }

        StartCoroutine(StartAfterTime());
        StartCoroutine(LimiterDestructionAfterTime());

        EnemyHealthBarBackGround = HealthBarGameObject.GetComponentsInChildren<Transform>()[1].gameObject;
        EnemyHealthBarFill = EnemyHealthBarBackGround.GetComponentsInChildren<Transform>()[1].gameObject;

        foreach (Transform t in transform.parent.GetComponentsInChildren<Transform>()) {
            if (t.gameObject.CompareTag("HealthBar")) {
                EnemyHealthBarBackGround = t.gameObject.GetComponentsInChildren<Transform>()[1].gameObject;

                EnemyHealthBarFill = EnemyHealthBarBackGround.GetComponentsInChildren<Transform>()[1].gameObject;
            }
        }

        EnemyHealthBarBackGround.transform.localScale = new Vector2(MaxHealth/100, EnemyHealthBarBackGround.transform.localScale.y);

        ChangeState(false);
    }

    IEnumerator StartAfterTime() {
        yield return new WaitForSeconds(AnimationStartDelay);
        ChangeState(true);
    }

    IEnumerator LimiterDestructionAfterTime() {
        yield return new WaitForSeconds(AnimationStartDelay + LimiterDestructionDelayAfterStart);
        LimiterDestruction = true;
        
    }

    void Update() {

        RotateTurret();
        LookForward();
        DestroyIfAnimationEnd();

        if (canShoot == true) {
            if (fireDelayTimeStamp <= Time.time) {
                Fire();

                fireDelayTimeStamp = Time.time + fireDelay;
            }
        }

        if (Health <= 0) {
            Health = 0;
            DestroyAndDropStuff(this.gameObject);
        }

        if (EnemyHealthBarBackGround != null) {
            
            EnemyHealthBarFill.transform.localScale = new Vector3(((100 / MaxHealth) * Health) * 0.01f, EnemyHealthBarFill.transform.localScale.y, EnemyHealthBarFill.transform.localScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (LimiterDestruction) {
            if (collision.gameObject.CompareTag("Enemy Limiter")) {
                Destroy(this.gameObject);
            }
        }
        if (noCollisionDamage == false) {
            if (collision.gameObject.CompareTag("Player")) {
                if (PlayerBehaviourScript.regenerates == true) {
                    PlayerBehaviourScript.regenerates = false;
                }
                collision.gameObject.GetComponent<PlayerBehaviourScript>().currendHealth -= CollisionDamage;
                StartCoroutine(GameControllerScript.ShakeMainCamera(0.2f, 0.05f));
            }
        }
    }

    void DestroyAndDropStuff(GameObject toDestroy) {

        GameControllerScript.currendScore += scoreValue;

        DropStuff();
        Destroy(toDestroy);
    }

    void Fire() {
        if (ShootType != ShootTypes.None) {
            if (WeaponProjectile.GetComponent<LaserBulletBehaviourScript>() != null) {

                switch (ShootType) {
                    case (ShootTypes.SingleBullet):
                        foreach (GameObject go in EnemyTurrets) {
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * go.transform.rotation * Quaternion.Euler(0, 0, 90));
                        }
                        break;
                    case (ShootTypes.FiveSpread):
                        float tempAngle = 10f;
                        foreach (GameObject go in EnemyTurrets) {
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * go.transform.rotation * Quaternion.Euler(0, 0, 90) * Quaternion.Euler(0, 0, 0));
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * go.transform.rotation * Quaternion.Euler(0, 0, 90) * Quaternion.Euler(0, 0, tempAngle));
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * go.transform.rotation * Quaternion.Euler(0, 0, 90) * Quaternion.Euler(0, 0, -tempAngle));
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * go.transform.rotation * Quaternion.Euler(0, 0, 90) * Quaternion.Euler(0, 0, tempAngle * 2));
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * go.transform.rotation * Quaternion.Euler(0, 0, 90) * Quaternion.Euler(0, 0, -tempAngle * 2));
                        }
                        break;
                    case (ShootTypes.DropBomb):
                        foreach (GameObject go in EnemyTurrets) {
                            Instantiate(WeaponProjectile, go.transform.position, WeaponProjectile.transform.rotation * Quaternion.Euler(0, 0, 0));
                        }
                        break;
                }
            }
            else {
                Debug.LogError("The WeaponProjectile in " + this.gameObject.name + " has no LaserBulletBehaviour Script attachted");
            }
            if (EnemyTurrets.Length == 0) {
                Debug.LogWarning("The Enemy " + gameObject.name + " tryed to fire but it has no turrets assinged");
            }
        }
    }

    void ChangeState(bool ChangeTo) {
        foreach (SpriteRenderer Sp in GetComponentsInChildren<SpriteRenderer>()) {
            Sp.enabled = ChangeTo;
        }

        foreach (Collider2D CiC2D in GetComponents<Collider2D>()) {
            CiC2D.enabled = ChangeTo;
        }

        /*
        foreach (BoxCollider2D BoC2D in GetComponents<BoxCollider2D>()) {
            BoC2D.enabled = ChangeTo;
        }
        foreach (CapsuleCollider2D CaC2D in GetComponents<CapsuleCollider2D>()) {
            CaC2D.enabled = ChangeTo;
        }
        */

        animator.enabled = ChangeTo; //starts/stops the animation
        EnemyHealthBarBackGround.SetActive(ChangeTo);
    }

    void DropStuff() {
        //Drop Credits
        int CoinDropAmount = Random.Range(minCoinDrop, maxCoinDrop + 1);
        for (int i = 1; i <= CoinDropAmount; i++) {
            Instantiate(ObjectHolder._Credits[ObjectHolder.GetCreditValueIndex(ValueOfCreditDrop)], transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        }

        //Drop PowerUp
        if (PowerUpDropChangse != 0) {
            if (Random.Range(0f, 1f) <= PowerUpDropChangse) {

                Debug.Log("Drop PowerUp");

                //PickUpBehaviourScript.PickUpTypes RandomPickUp = (PickUpBehaviourScript.PickUpTypes)Random.Range(1/*cus 0 is credit*/, ObjectHolder._PowerUps.Length - 1);


                Instantiate(ObjectHolder._PowerUps[Random.Range(0, ObjectHolder._PowerUps.Length)], transform.position, Quaternion.identity);
            }
        }
    }

    void RotateTurret() {
        if (aimTurrets == true) {
            if (GameObject.FindGameObjectWithTag("Player") != null) {
                foreach (GameObject go in EnemyTurrets) {
                    if (go != null) {
                        go.transform.right = GameObject.FindGameObjectWithTag("Player").transform.position - go.transform.position;
                    }
                    else Debug.LogWarning("The enemy " + gameObject.name + " has a null element in the Turret array.");
                }
            }
        }
    }

    void DestroyIfAnimationEnd() {
        if (destroyAfterAnimation == true) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) { //(I think: ) normelized time is the percentige of the time so if it is over 1 the animation is also over
                Destroy(this.gameObject);
            }
        }
    }

    void LookForward() {
        if (lookForwardWhenMoving == true) {
            if (Time.timeScale != 0) {
                Vector3 lookDirection = transform.position - lastFramePos;
                Vector3.Normalize(lookDirection);

                transform.up = Quaternion.Inverse(transform.parent.rotation) * (startRotation * -lookDirection);

                lastFramePos = transform.position;
            }
        }
    }
}