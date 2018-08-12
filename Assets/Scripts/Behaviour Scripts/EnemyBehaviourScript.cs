using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour {

    public GameObject EnemyTurretGameObject;
    public GameObject ObjectHolderGo;
    //public GameObject GameControllerGo;
    public GameObject CoinDrop;
    public GameObject PowerUpDrop;
    private GameObject EnemyHealthBarRed;
    private GameObject EnemyHealthBarGreen;

    private GameObject[] EnemyBullets;

    private Animator animator;

    public enum EnemyTypes { AlienStandart, AlienTurret, AlienHeavy }
    public EnemyTypes currendEnemyType = EnemyTypes.AlienStandart;

    public enum EnemyWeapons { None, FastSmall, FiveSpreadSlow}
    public EnemyWeapons EnemyWeapon = EnemyWeapons.FastSmall;

    public enum AnimationTypes {
        DoNotMove, StraightDown, ComeInFromRight, ComeInFromLeft, ComeDownMiddleGoUpRight, ComeDownMiddleGoUpLeft, StraightDownBoolShoot3, DownWaitUp, DownShoot2Up, HalfCircleRightLeftShoot2,
        RightToLeftShoot5, RightGoMiddleUpShoot3, MovingLeftTurn180Shoot4, DownDeaccAcc, DownDeaccShoot1Acc, DownStrave_RightFirst_, DownStraveSmall_RightFirst_, GoToBottomShoot6
    }
    public AnimationTypes currendAnimation;
    public float AnimationStartDelay = 0f;
    private float AnimationStartDelayTimeStamp;
    //private bool checkForAnimationDelay = true;

    //only Temp
    public bool hasHealthBar = false;
    public bool hasAnimation = false;
    public bool hasTurret = false;

    public bool canShoot = false;

    /*--------------------Stats--------------------*/
    public float MaxHealth = 100f;
    public float Health = 100f;
    public float CollisionDamage = 50f;

    //public Vector3 direction = new Vector3(1, 0);
    //private float movementspeed = 0.1f;

    //PowerUp Variables
    public static bool noCollisionDamage = false;

    /*---------------------------------------------End-Of-Variables---------------------------------------------------------------------------*/
    void Start() {
        //AnimationStartDelayTimeStamp = Time.time + AnimationStartDelay;
        EnemyBullets = ObjectHolderGo.GetComponent<ObjectHolder>().EnemyBullets;

        StartCoroutine(StartAfterTime());
        //AnimationStartDelayTimeStamp = Time.time + AnimationStartDelay;

        if (hasHealthBar) {
            if (GetComponentsInChildren<Transform>()[1].gameObject.CompareTag("HealthBar")) {
                EnemyHealthBarRed = GetComponentsInChildren<Transform>()[1].gameObject;

                EnemyHealthBarGreen = EnemyHealthBarRed.GetComponentsInChildren<Transform>()[1].gameObject;
            }
        }
        /*
        if (hasHealthBar) {
            for (int i = 0; i < GetComponentsInChildren<Transform>().Length; i++) {
                if (GetComponentsInChildren<Transform>()[i].gameObject.CompareTag("HealthBar")) {
                    EnemyHealthBarRed = GetComponentsInChildren<Transform>()[i].gameObject;

                    EnemyHealthBarGreen = EnemyHealthBarRed.GetComponentsInChildren<Transform>()[1].gameObject;
                }
            }
        }
        */
        if (hasAnimation) {
            animator = GetComponent<Animator>();
        }
    }

    IEnumerator StartAfterTime() {
        yield return new WaitForSeconds(AnimationStartDelay);
        startAnimation();
    }

    void Update() {


        /* Different Wait Type
        if (AnimationStartDelayTimeStamp <= Time.time) {
            startAnimation();
        }
        */


        if (hasTurret) {
            EnemyTurretGameObject.transform.right = GameObject.FindGameObjectsWithTag("Player")[0].transform.position - transform.position;
        }

        if (canShoot == true) {
            Fire();
            canShoot = false;
        }

        if (Health <= 0) {
            StartSelfDestruction(this.gameObject);
        }
        if (hasHealthBar) {
            EnemyHealthBarGreen.transform.localScale = new Vector3(((100 / MaxHealth) * Health) * 0.01f, EnemyHealthBarGreen.transform.localScale.y, EnemyHealthBarGreen.transform.localScale.z);
        }
    }

    void FixedUpdate() {
        //transform.Translate(direction * movementspeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy Limiter")) {
            Destroy(this.gameObject);
        }
        if (noCollisionDamage == false) {
            if (collision.gameObject.CompareTag("Player")) {
                if (PlayerControllerScript.regenerates == true) {
                    PlayerControllerScript.regenerates = false;
                }
                collision.gameObject.GetComponent<PlayerControllerScript>().currendHealth -= CollisionDamage;
                StartCoroutine(GameControllerScript.ShakeMainCamera(0.2f, 0.05f));
            }
        }
        if (collision.gameObject.CompareTag("Explosion")) {
            Health -= collision.gameObject.GetComponent<LaserBulletData>().damage;
        }
    }

    void StartSelfDestruction(GameObject toDestroy) {

        Instantiate(CoinDrop, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Instantiate(CoinDrop, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Instantiate(CoinDrop, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Destroy(toDestroy);
    }

    void startAnimation() {
        switch (currendAnimation) {
            case (AnimationTypes.StraightDown):
                animator.SetBool("StraightDownBool", true);
                break;
            case (AnimationTypes.ComeInFromRight):
                animator.SetBool("ComeInFromRightBool", true);
                break;
            case (AnimationTypes.ComeInFromLeft):
                animator.SetBool("ComeInFromLeftBool", true);
                break;
            case (AnimationTypes.ComeDownMiddleGoUpRight):
                animator.SetBool("ComeDownMiddleGoUpRightBool", true);
                break;
            case (AnimationTypes.ComeDownMiddleGoUpLeft):
                animator.SetBool("ComeDownMiddleGoUpLeftBool", true);
                break;
            case (AnimationTypes.StraightDownBoolShoot3):
                animator.SetBool("StraightDownBoolShoot3Bool", true);
                break;
            case (AnimationTypes.DownWaitUp):
                animator.SetBool("DownWaitUpBool", true);
                break;
            case (AnimationTypes.DownShoot2Up):
                animator.SetBool("DownShoot2UpBool", true);
                break;
            case (AnimationTypes.HalfCircleRightLeftShoot2):
                animator.SetBool("HalfCircleRightLeftShoot2Bool", true);
                break;
            case (AnimationTypes.RightToLeftShoot5):
                animator.SetBool("RightToLeftShoot5Bool", true); 
                break;
            case (AnimationTypes.RightGoMiddleUpShoot3):
                animator.SetBool("RightGoMiddleUpShoot3Bool", true);
                break;
            case (AnimationTypes.MovingLeftTurn180Shoot4):
                animator.SetBool("MovingLeftTurn180Shoot4Bool", true);
                break;
            case (AnimationTypes.DownDeaccAcc):
                animator.SetBool("DownDeaccAccBool", true);
                break;
            case (AnimationTypes.DownDeaccShoot1Acc):
                animator.SetBool("DownDeaccShoot1AccBool", true);
                break;
            case (AnimationTypes.DownStrave_RightFirst_):
                animator.SetBool("DownStrave_RightFirst_Bool", true);
                break;
            case (AnimationTypes.DownStraveSmall_RightFirst_):
                animator.SetBool("DownStraveSmall_RightFirst_Bool", true);
                break;
            case (AnimationTypes.GoToBottomShoot6):
                animator.SetBool("GoToBottomShoot6Bool", true);
                break;
                //
        }
    }

    void Fire() {
        switch (EnemyWeapon) {
            case (EnemyWeapons.FastSmall): //Must have a Turret
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SimpleBullet)], EnemyTurretGameObject.transform.position, EnemyTurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 90));
                break;
            case (EnemyWeapons.FiveSpreadSlow):
                float tempAngle = 10f;
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, 0));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, tempAngle));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, -tempAngle));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, tempAngle * 2));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, -tempAngle * 2));
                break;
        }
    }

    void ChangeState(bool ChangeTo) {
        GetComponent<SpriteRenderer>().enabled = ChangeTo;
        if (GetComponent<CircleCollider2D>() != null) {
            GetComponent<CircleCollider2D>().enabled = ChangeTo;
        }
        if (GetComponent<BoxCollider2D>() != null) {
            GetComponent<BoxCollider2D>().enabled = ChangeTo;
        }
        if (GetComponent<CapsuleCollider2D>() != null) {
            GetComponent<CapsuleCollider2D>().enabled = ChangeTo;
        }
    }
}
