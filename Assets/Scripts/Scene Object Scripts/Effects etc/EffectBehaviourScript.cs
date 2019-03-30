using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviourScript : MonoBehaviour {

    public enum EffectTypes { LaserLoaded, BulletDestruction, SniperCharging }
    public EffectTypes effectType;

    private float TimeStamp;
    public float TimeToWait = 0.1f;

    void Start () {
        TimeStamp = Time.time + TimeToWait;
    }
	
	void Update () {
        if (TimeStamp <= Time.time) {
            Destroy(this.gameObject);
        }
    }
}
