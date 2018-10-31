using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour {

    public GameObject ObjectHolderGo;
    public GameObject[] EnemyTurrets = new GameObject[0];

    private GameObject EnemyHealthBarRed;
    private GameObject EnemyHealthBarGreen;
    private Animator animator;

    private GameObject[] EnemyBullets;


    public enum EnemyTypes { AlienStandart, AlienTurret, AlienHeavy, AilenWingShip_straight, AilenWingShip_aim }
    public EnemyTypes currendEnemyType = EnemyTypes.AlienStandart;

    public enum EnemyWeapons { None, FastSmall_aim, FiveSpreadSlow_straight, FourSmallLaserBullets_straight }
    public EnemyWeapons EnemyWeapon = EnemyWeapons.None;

    //Start Delays
    [SerializeField]
    private float AnimationStartDelay = 0f;
    [SerializeField]
    private float LimiterDestructionDelayAfterStart = 1f;
    private bool LimiterDestruction = false;

    //the movement/behaviour animation change this value for one frame which will make the enemy shoot in that frame
    public bool canShoot = false; //Has to be Serializable to be able to be changed by an animation


    /*--------------------Stats--------------------*/
    public float MaxHealth = 100f;
    public float Health = 100f;
    public float CollisionDamage = 50f;


    /*--------------------DropStuff--------------------*/
    public float ValueOfCreditDrop = 1f; 
    int maxCoinDrop = 3;
    int minCoinDrop = 2;
    public float PowerUpDropChangse = 0f; //0 for nothing (whaaat?)

    //PowerUp Variables
    public static bool noCollisionDamage = false;

    /*---------------------------------------------End-Of-Variables---------------------------------------------------------------------------*/
    void Start() {
        EnemyBullets = ObjectHolderGo.GetComponent<ObjectHolder>().EnemyBullets;
        if (GetComponent<Animator>() != null) {
            animator = GetComponent<Animator>();
        }

        StartCoroutine(StartAfterTime());
        StartCoroutine(LimiterDestructionAfterTime());

        foreach (Transform t in transform.parent.GetComponentsInChildren<Transform>()) {
            if (t.gameObject.CompareTag("HealthBar")) {
                EnemyHealthBarRed = t.gameObject.GetComponentsInChildren<Transform>()[1].gameObject;

                EnemyHealthBarGreen = EnemyHealthBarRed.GetComponentsInChildren<Transform>()[1].gameObject;
            }
        }

        EnemyHealthBarRed.transform.localScale = new Vector2(MaxHealth/1000, EnemyHealthBarRed.transform.localScale.y);

        ChangeState(false);
    }

    IEnumerator StartAfterTime() {
        yield return new WaitForSeconds(AnimationStartDelay);
        ChangeState(true);
    }

    IEnumerator LimiterDestructionAfterTime() {
        yield return new WaitForSeconds(LimiterDestructionDelayAfterStart + AnimationStartDelay);
        LimiterDestruction = true;
        
    }

    void Update() {
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            foreach (GameObject go in EnemyTurrets) {
                if (go != null) {
                    go.transform.right = GameObject.FindGameObjectWithTag("Player").transform.position - go.transform.position;
                }
                else Debug.LogWarning("The enemy " + gameObject.name + " has a null element in the Turret array.");
            }
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
                if (PlayerBehaviourScript.regenerates == true) {
                    PlayerBehaviourScript.regenerates = false;
                }
                collision.gameObject.GetComponent<PlayerBehaviourScript>().currendHealth -= CollisionDamage;
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

    void Fire() {
        switch (EnemyWeapon) {
            case (EnemyWeapons.FastSmall_aim): //Must have a Turret
                foreach (GameObject go in EnemyTurrets) {
                    Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SimpleBullet)], go.transform.position, go.transform.rotation * Quaternion.Euler(0, 0, 90));
                }
                break;
            case (EnemyWeapons.FiveSpreadSlow_straight):
                float tempAngle = 10f;
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, 0));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, tempAngle));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, -tempAngle));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, tempAngle * 2));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_SlowAlienBullet)], transform.position, transform.rotation * Quaternion.Euler(0, 0, -tempAngle * 2));
                break;
            case (EnemyWeapons.FourSmallLaserBullets_straight):
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_AlienLaserBulletSmall)], transform.position + new Vector3(1.3f, -0.525f, 0), transform.rotation * Quaternion.Euler(0, 0, 0));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_AlienLaserBulletSmall)], transform.position + new Vector3(1f, -0.7f, 0), transform.rotation * Quaternion.Euler(0, 0, 0));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_AlienLaserBulletSmall)], transform.position + new Vector3(-1.3f, -0.525f, 0), transform.rotation * Quaternion.Euler(0, 0, 0));
                Instantiate(EnemyBullets[ObjectHolder.GetEnemyBulletIndex(LaserBulletData.BulletTypes.Enemy_AlienLaserBulletSmall)], transform.position + new Vector3(-1f, -0.7f, 0), transform.rotation * Quaternion.Euler(0, 0, 0));
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

        animator.enabled = ChangeTo; //starts/stops the animation
        EnemyHealthBarRed.SetActive(ChangeTo);
    }

    void DropStuff() {
        //Drop Credits
        int CoinDropAmount = Random.Range(minCoinDrop, maxCoinDrop + 1);
        for (int i = 1; i <= CoinDropAmount; i++) {
            Instantiate(ObjectHolder._Credits[ObjectHolder.GetCreditValueIndex(ValueOfCreditDrop)], transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        }

        //Drop PowerUp
        if (PowerUpDropChangse == 0) {
            if (Random.Range(0f, 1f) <= PowerUpDropChangse) {

                Debug.Log("Drop PowerUp");

                PickUpBehaviourScript.PickUpTypes RandomPickUp = (PickUpBehaviourScript.PickUpTypes)Random.Range(1/*cus 0 is credit*/, ObjectHolder._PowerUps.Length);
                Instantiate(ObjectHolder._PowerUps[ObjectHolder.GetPowerUpIndex(RandomPickUp)], transform.position, Quaternion.identity);
            }
        }
    }
}
