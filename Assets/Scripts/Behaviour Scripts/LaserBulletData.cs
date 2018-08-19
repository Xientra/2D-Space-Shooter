﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletData : MonoBehaviour {

    public GameObject explosion;

    public enum BulletTypes {
        Standart, HelixBullet_lvl_1, HelixBullet_lvl_2, HelixBullet_lvl_3, HelixBulletChild, Wave, SniperBullet, Rocket, Grenade, Shrapnel, ChainGunBullet, ExplosionSmall,
        SimpleLaser, SplitLaser, SplitLaserChild, 
        Enemy_SimpleBullet, Enemy_SlowAlienBullet
    }
    public BulletTypes bulletType = BulletTypes.Standart;

    public Vector3 direction = new Vector3(1, 0, 0);

    private LineRenderer lineRenderer;

    private bool isLaser = false;
    [SerializeField]
    private bool isEnemyBullet = false;

    [SerializeField]
    private float HelixBulletChild_RotationSpeed = 10f;

    /*--------------------Stats--------------------*/
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    public float damage = 10f;
    [SerializeField]
    private float duration = 1f;
    //[SerializeField]
    private float DelayDestructionTime = 0.25f; //this should be at least as long as the camera is shaken on hit (0.2f)
    //private float cooldown = 0.5f;
    //private enum SpecialEffects { none };
    //[SerializeField]
    //private SpecialEffects SpecialEffect = SpecialEffects.none;

    private float damageDelay = 0.1f; //this is kinda important for the damage of Lasers-------------------------------------------------------------------
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

        if (bulletType == BulletTypes.Enemy_SimpleBullet || bulletType == BulletTypes.Enemy_SlowAlienBullet) {
            isEnemyBullet = true;
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
        Destroy(this.gameObject);
        //InitiliseSelfDestruction();
    }

    void FixedUpdate() {
        if (isLaser == false) {
            transform.Translate(direction * speed * Time.deltaTime); //Moving
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isEnemyBullet != true) {
            if (isLaser == false) {
                if (collision.gameObject.layer == 10) {
                    collision.gameObject.GetComponent<EnemyBehaviourScript>().Health -= damage * damageMultiplyer;
                    InitiliseSelfDestruction();
                }
            }
        }
        else {
            if (isLaser == false) {
                if (collision.gameObject.CompareTag("Player")) {
                    collision.gameObject.GetComponent<PlayerControllerScript>().currendHealth -= damage;
                    StartCoroutine(GameControllerScript.ShakeMainCamera(0.2f, 0.05f));
                    InitiliseSelfDestruction();
                }
            }
        }
        if (collision.gameObject.layer == 8/*Static*/) {
            InitiliseSelfDestruction();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) { //this is should be framerate indipendend
        if (isEnemyBullet != true) {
            if (isLaser == true) {
                if (collision.gameObject.layer == 10) {
                    if (damageDelayTimeStamp <= Time.time) {
                        collision.gameObject.GetComponent<EnemyBehaviourScript>().Health -= damage * damageMultiplyer;
                        damageDelayTimeStamp = Time.time + damageDelay;
                    }
                }
            }
        }
        else { }
    }

    private void OnDestroy() {
    }

    void InitiliseSelfDestruction() {
        //Instantiate explosion is needed
        if (bulletType == BulletTypes.Rocket) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else
        if (bulletType == BulletTypes.Grenade) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else
        if (bulletType == BulletTypes.Shrapnel) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else {
            //deactivate Collider
            if (GetComponent<CircleCollider2D>() != null) {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            if (GetComponent<BoxCollider2D>() != null) {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            if (GetComponent<CapsuleCollider2D>() != null) {
                GetComponent<CapsuleCollider2D>().enabled = false;
            }

            StartCoroutine(DoStuffAfterOneFrame());

            speed = speed / 10f;
            Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.BulletDestruction)], transform);

            StartCoroutine(DelayDestruction());
        }
    }

    IEnumerator DelayDestruction() {
        yield return new WaitForSeconds(DelayDestructionTime);
        Destroy(this.gameObject);
    }

    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;

        foreach (SpriteRenderer SR in GetComponentsInChildren<SpriteRenderer>()) {
            SR.enabled = false;
        }
        
        foreach (TrailRenderer TR in GetComponentsInChildren<TrailRenderer>()) {
            TR.enabled = false;
        }
        
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