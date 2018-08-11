using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour {

    public GameObject[] Bullets;
    static GameObject[] _Bullets;

    public GameObject[] EnemyBullets;
    static GameObject[] _EnemyBullets;

    public GameObject[] PowerUps;
    static GameObject[] _PowerUps;

    public GameObject[] Effects;
    //static GameObject[] _Effects;
    public enum EffectNames { LaserLoaded }
    public EffectNames effectName;

    void Start() {
        _Bullets = new GameObject[Bullets.Length];
        _Bullets = Bullets;

        _EnemyBullets = new GameObject[EnemyBullets.Length];
        _EnemyBullets = EnemyBullets;

        _PowerUps = new GameObject[PowerUps.Length];
        _PowerUps = PowerUps;

        //_Effects = new GameObject[Effects.Length];
        //_Effects = Effects;
    }
    /*
    void Update() {
    }
    */

    public static int GetBulletIndex(LaserBulletData.BulletTypes _bulletType) {
        int i = 0;
        foreach (GameObject go in _Bullets) {
            if (go != null) {
                if (go.GetComponentInChildren<LaserBulletData>().bulletType == _bulletType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _bulletType);
        return -1;
    }

    public static int GetEnemyBulletIndex(LaserBulletData.BulletTypes _EnemyBulletType) {
        int i = 0;
        foreach (GameObject go in _EnemyBullets) {
            if (go != null) {
                if (go.GetComponentInChildren<LaserBulletData>().bulletType == _EnemyBulletType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _EnemyBulletType);
        return -1;
    }

    public static int GetPowerUpIndex(PowerUpBehaviourScript.PowerUpTypes _PowerUpType) {
        int i = 0;
        foreach (GameObject go in _PowerUps) {
            if (go != null) {
                if (go.GetComponentInChildren<PowerUpBehaviourScript>().currendPowerUpType == _PowerUpType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _PowerUpType);
        return -1;
    }

    public int GetEffectIndex(EffectNames _effectType) {
        int i = 0;
        foreach (GameObject go in Effects) {
            if (go != null) {
                if (effectName == _effectType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _effectType);
        return -1;
    }
}
