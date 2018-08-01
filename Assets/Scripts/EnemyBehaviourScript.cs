using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour {

    public GameObject EnemyTurretGameObject;
    public GameObject CoinDrop;
    public GameObject PowerUpDrop;
    private GameObject EnemyHealthBarRed;
    private GameObject EnemyHealthBarGreen;

    private Animator animator;

    public enum EnemyTypes { Standart, StandartShoot }
    public EnemyTypes currendEnemyType = EnemyTypes.Standart;

    public enum AnimationTypes { DoNotMove, StraightDown, ComeInFromRight, ComeInFromLeft, ComeDownMiddleGoUpRight, ComeDownMiddleGoUpLeft }
    public AnimationTypes currendAnimation;
    public float AnimationStartDelay = 0f;
    //private float AnimationStartDelayTimeStamp;
    private bool checkForAnimationDelay = true;

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

        StartCoroutine(StartAfterTime());
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

        EnemyTurretGameObject.transform.right = GameObject.FindGameObjectsWithTag("Player")[0].transform.position - transform.position;

        /*
        if (checkForAnimationDelay) {
            if (AnimationStartDelayTimeStamp < Time.time) {
                startAnimation();
            }
        }
        */
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
