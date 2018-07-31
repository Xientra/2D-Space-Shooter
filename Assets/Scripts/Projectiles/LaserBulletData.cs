using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletData : MonoBehaviour {

    public GameObject explosion;

    public enum BulletTypes {
        Standart, HelixBullet_lvl_1, HelixBullet_lvl_2, HelixBullet_lvl_3, HelixBulletChild, Wave, SniperBullet, Rocket, Grenade, Shrapnel, ChainGunBullet, ExplosionSmall,
        SimpleLaser, SplitLaser, SplitLaserChild
    }
    public BulletTypes bulletType = BulletTypes.Standart;

    public Vector3 direction = new Vector3(1, 0);

    private LineRenderer lineRenderer;

    private bool isLaser = false;
    [SerializeField]
    private float HelixBulletChild_RotationSpeed = 10f;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float duration = 1f;
    //private float cooldown = 0.5f;
    [SerializeField]
    public float damage = 10f;
    //[SerializeField]
    //private enum SpecialEffects { none };
    //[SerializeField]
    //private SpecialEffects SpecialEffect = SpecialEffects.none;
    
    private float damageDelay = 0.1f; //this is kinda important for the Lasers-------------------------------------------------------------------
    private float damageDelayTimeStamp;
    
    
    //for Split Laser
    private static int timesToSplit = 1;
    private float SplitAngle = 25f;

    //PowerUp Variables
    public static float damageMultiplyer = 1f;

    void Start() {
        if (bulletType == BulletTypes.SimpleLaser || bulletType == BulletTypes.SplitLaser) {
            isLaser = true;
            damageDelayTimeStamp = Time.time + damageDelay;
        }

        if (bulletType == BulletTypes.SplitLaserChild) {
            transform.rotation *= Quaternion.Euler(0, 0, Random.Range(SplitAngle, -SplitAngle));
        }
    }

    void Update() {
        StartCoroutine(destroyAfterTime());

        if (bulletType == BulletTypes.HelixBulletChild) {
            this.transform.RotateAround(transform.parent.transform.position, Vector3.forward, HelixBulletChild_RotationSpeed);
        }
    }

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(duration);
        InitiliseSelfDestruction();
    }

    void FixedUpdate() {
        if (isLaser == false) {
            transform.Translate(direction * speed); //Moving
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isLaser == false) {
            if (collision.gameObject.layer == 10) {
                collision.gameObject.GetComponent<EnemyBehaviourScript>().Health -= damage * damageMultiplyer;
                InitiliseSelfDestruction();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) { //dude this is not framerate indipendend or is it?
        if (isLaser == true) {
            if (collision.gameObject.layer == 10) {
                if (damageDelayTimeStamp <= Time.time) {
                    collision.gameObject.GetComponent<EnemyBehaviourScript>().Health -= damage * damageMultiplyer;
                    damageDelayTimeStamp = Time.time + damageDelay;
                }
            }
        }
    }


    private void OnDestroy() {
        if (bulletType == BulletTypes.Rocket) {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (bulletType == BulletTypes.Grenade) {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (bulletType == BulletTypes.Shrapnel) {
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    void InitiliseSelfDestruction() {
        Destroy(this.gameObject);
    }

    void startSplitLaser(float lengthOfParent) {
        if (timesToSplit > 0) {
            GameObject goParent = new GameObject("SplitLaserHolder_" + timesToSplit.ToString());
            goParent.transform.localScale /= 2;
            Instantiate(goParent, transform.position + Vector3.Cross(transform.right, new Vector3(lengthOfParent, 0, 0)), transform.rotation * Quaternion.Euler(0, 0, Random.Range(SplitAngle, -SplitAngle)), this.transform.gameObject.transform);

            //Instantiate(this.transform.gameObject, transform.position + Vector3.Cross(transform.right, new Vector3(0.75f, 0, 0)), goParent.transform.rotation, goParent.transform);
            timesToSplit--;
        }
    }
}