using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletScript : MonoBehaviour {

    public Sprite[] Bullet_Sprites;

    public enum BulletTypes { Standart, HelixBullet, HelixBulletChild, Wave, SniperBullet }
    public BulletTypes type = BulletTypes.Standart;

    public Vector3 direction = new Vector3(1, 0);

    public float HelixBulletChild_RotationSpeed = 60f;

    private float speed = 1f;
    private float duration = 1f;
    //private float cooldown = 0.5f;
    [NonSerialized]
    public float damage = 10f;
    private enum SpecialEffects { none };
    private SpecialEffects SpecialEffect = SpecialEffects.none;
    //scale x = 0.3f
    //sacle y = 0.3f
    //sprite/collidor
    //trail length

    void Start () {

        switch (type) {
            case (BulletTypes.Standart):
                speed = 1f;
                duration = 1f;
                damage = 20f;
                adjustScale(0.3f, 0.3f);
                break;
            case (BulletTypes.HelixBullet):
                speed = 0.3f;
                duration = 1f;
                damage = 20f;
                adjustScale(0.25f, 0.25f);
                break;
            case (BulletTypes.HelixBulletChild):
                speed = 0f;
                duration = 1f;
                damage = 30f;
                //adjust scale
                transform.localScale = new Vector3(0.7f, 0.7f, transform.localScale.z); //relativ to it's parent HelixBullet
                this.gameObject.GetComponent<TrailRenderer>().startWidth = 0.10f / 3f;

                break;
            case (BulletTypes.Wave):
                speed = 0.20f;
                duration = 0.25f;
                damage = 50f;
                GetComponent<SpriteRenderer>().sprite = Bullet_Sprites[3];
                
                adjustScale(1f, 1f);
                break;
            case (BulletTypes.SniperBullet):
                speed = 2f;
                duration = 0.2f;
                damage = 200f;
                adjustScale(0.5f, 0.25f);
                break;
            default:
                Debug.LogError("The Bullet Type -" + type.ToString() + "- has no values assinged!");
                break;
        }
    }
	
	void Update () {
        StartCoroutine(destroyAfterTime());

        if (type == BulletTypes.HelixBulletChild) {
            this.transform.RotateAround(transform.parent.transform.position, Vector3.forward, HelixBulletChild_RotationSpeed);
        }
    }

    IEnumerator destroyAfterTime() {                                                                            
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

    void FixedUpdate() {
        transform.Translate(direction * speed);
    }

    void adjustScale(float xscale, float yscale) {
        transform.localScale = new Vector3(xscale, yscale, transform.localScale.z);
        this.gameObject.GetComponent<TrailRenderer>().startWidth = yscale / 3;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 10) {
        }
    }
}