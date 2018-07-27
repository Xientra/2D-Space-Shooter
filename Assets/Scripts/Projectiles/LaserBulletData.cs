using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletData : MonoBehaviour {

    public GameObject explosion;

    public enum BulletTypes {
        Standart, HelixBullet_lvl_1, HelixBullet_lvl_2, HelixBullet_lvl_3, HelixBulletChild, Wave, SniperBullet, Rocket, Grenade, Shrapnel, ChainGunBullet, ExplosionSmall,
        SimpleLaser
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
    
    private float damageDelay = 0.05f; //this is kinda important for the Lasers-------------------------------------------------------------------
    [SerializeField]
    private float damageDelayTimeStamp;

    //PowerUp Variables
    public static float damageMultiplyer = 1f;

    void Start() {
        if (bulletType == BulletTypes.SimpleLaser) {
            isLaser = true;
            damageDelayTimeStamp = Time.time + damageDelay;
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
}