using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour {

    public GameObject CoinDrop;
    public GameObject PowerUpDrop;
    public GameObject EnemyHealthBarRed;
    public GameObject EnemyHealthBarGreen;

    public enum EnemyTypes { Standart }
    public EnemyTypes currendEnemyType = EnemyTypes.Standart;

    public bool hasHealthBar = false;

    public float MaxHealth = 100f;
    public float Health = 100f;
    public float CollisionDamage = 50f;

    public Vector3 direction = new Vector3(1, 0);

    private float movementspeed = 0.1f;

    //PowerUp Variables
    public static bool noCollisionDamage = false;


    void Start() {

        if (hasHealthBar) {
            EnemyHealthBarRed = GetComponentsInChildren<Transform>()[1].gameObject;
            EnemyHealthBarGreen = EnemyHealthBarRed.GetComponentsInChildren<Transform>()[1].gameObject;
        }
    }

    void Update() {
        if (Health <= 0) {
            StartSelfDestruction(this.gameObject);
        }
        if (hasHealthBar) {
            EnemyHealthBarGreen.transform.localScale = new Vector3(((100 / MaxHealth) * Health) * 0.01f, EnemyHealthBarGreen.transform.localScale.y, EnemyHealthBarGreen.transform.localScale.z);
        }
    }

    void FixedUpdate() {
        transform.Translate(direction * movementspeed * Time.deltaTime);
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
        /*
        if (collision.gameObject.CompareTag("Projectile")) {
            if (collision.gameObject.GetComponent<LaserBulletScript>() != null) {
                Health -= collision.gameObject.GetComponent<LaserBulletScript>().damage;
            }
            else {
                Health -= collision.gameObject.GetComponent<LaserBulletData>().damage;
            }
            Destroy(collision.gameObject);
        }
        */
    }

    void StartSelfDestruction(GameObject toDestroy) {

        Instantiate(CoinDrop, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Instantiate(CoinDrop, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Instantiate(CoinDrop, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        Destroy(toDestroy);
    }

    void changeEnemyType() {
        switch (currendEnemyType) {
            case (EnemyTypes.Standart):



                break;
        }
    }
}
