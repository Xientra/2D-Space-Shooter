using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour {

    public GameObject EnemyTurretGameObject;
    public GameObject ObjectHolderGo;
    //public GameObject GameControllerGo;
    private GameObject EnemyHealthBarRed;
    private GameObject EnemyHealthBarGreen;
    private Animator animator;

    private GameObject[] EnemyBullets;

    public enum EnemyTypes { AlienStandart, AlienTurret, AlienHeavy }
    public EnemyTypes currendEnemyType = EnemyTypes.AlienStandart;

    public enum EnemyWeapons { None, FastSmall, FiveSpreadSlow}
    public EnemyWeapons EnemyWeapon = EnemyWeapons.FastSmall;

    public enum AnimationTypes {
        DoNotMove, StraightDown, ComeInFromRight, ComeInFromLeft, ComeDownMiddleGoUpRight, ComeDownMiddleGoUpLeft, StraightDownBoolShoot3, DownWaitUp, DownShoot2Up, HalfCircleRightLeftShoot2,
        RightToLeftShoot5, RightGoMiddleUpShoot3, MovingLeftTurn180Shoot4, DownDeaccAcc, DownDeaccShoot1Acc, DownStrave_RightFirst_, DownStraveSmall_RightFirst_, GoToBottomShoot6
    }
    public AnimationTypes currendAnimation;

    [SerializeField]
    private  float AnimationStartDelay = 0f;
    //private float AnimationStartDelayTimeStamp;
    //private bool checkForAnimationDelay = true;

    [SerializeField]
    private float LimiterDestructionDelayAfterStart = 1f;
    private bool LimiterDestruction = false;

    //only Temp
    public bool hasTurret = false;

    public bool canShoot = false; //Has to be Serializable to be able to be changed by an animation

    /*--------------------Stats--------------------*/
    public float MaxHealth = 100f;
    public float Health = 100f;
    public float CollisionDamage = 50f;

    /*--------------------DropStuff--------------------*/
    public float ValueOfCreditDrop = 1f;
    public bool CanDropPowerUp;
    public float PowerUpDropChangse = 0.2f;

    //PowerUp Variables
    public static bool noCollisionDamage = false;

    /*---------------------------------------------End-Of-Variables---------------------------------------------------------------------------*/
    void Start() {
        EnemyBullets = ObjectHolderGo.GetComponent<ObjectHolder>().EnemyBullets;

        StartCoroutine(StartAfterTime());
        StartCoroutine(LimiterDestructionAfterTime());
        //AnimationStartDelayTimeStamp = Time.time + AnimationStartDelay;

        foreach (Transform t in GetComponentsInChildren<Transform>()) {
            if (t.gameObject.CompareTag("HealthBar")) {
                EnemyHealthBarRed = t.gameObject;

                EnemyHealthBarGreen = EnemyHealthBarRed.GetComponentsInChildren<Transform>()[1].gameObject;
            }
        }
        if (GetComponent<Animator>() != null) {
            animator = GetComponent<Animator>();
        }

        ChangeState(false);
    }

    IEnumerator StartAfterTime() {
        yield return new WaitForSeconds(AnimationStartDelay);
        ChangeState(true);
        startAnimation();
    }

    IEnumerator LimiterDestructionAfterTime() {
        yield return new WaitForSeconds(LimiterDestructionDelayAfterStart + AnimationStartDelay);
        LimiterDestruction = true;
        
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
        if (EnemyHealthBarRed != null) {
            EnemyHealthBarGreen.transform.localScale = new Vector3(((100 / MaxHealth) * Health) * 0.01f, EnemyHealthBarGreen.transform.localScale.y, EnemyHealthBarGreen.transform.localScale.z);
        }
    }
    /*
    void FixedUpdate() {
        //transform.Translate(direction * movementspeed * Time.deltaTime);
    }
    */
    void OnTriggerEnter2D(Collider2D collision) {
        if (LimiterDestruction) {
            if (collision.gameObject.CompareTag("Enemy Limiter")) {
                Destroy(this.gameObject);
            }
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

        DropStuff();
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
        foreach (SpriteRenderer Sp in GetComponentsInChildren<SpriteRenderer>()) {
            Sp.enabled = ChangeTo;
        }

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

    void DropStuff() {
        //Drop Credits
        Instantiate(ObjectHolder._Credits[ObjectHolder.GetCreditValueIndex(ValueOfCreditDrop)], transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Instantiate(ObjectHolder._Credits[ObjectHolder.GetCreditValueIndex(ValueOfCreditDrop)], transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Instantiate(ObjectHolder._Credits[ObjectHolder.GetCreditValueIndex(ValueOfCreditDrop)], transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));

        //Drop PowerUp
        if (CanDropPowerUp == true) {
            if (Random.Range(0f, 1f) <= PowerUpDropChangse) {

                Debug.Log("Drop PowerUp");

                PickUpBehaviourScript.PickUpTypes RandomPickUp = (PickUpBehaviourScript.PickUpTypes)Random.Range(0, ObjectHolder._PowerUps.Length);
                Instantiate(ObjectHolder._PowerUps[ObjectHolder.GetPowerUpIndex(RandomPickUp)], transform.position, Quaternion.identity);
            }
        }
    }
}
