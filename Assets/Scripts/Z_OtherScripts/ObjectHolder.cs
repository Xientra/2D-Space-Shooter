using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour {

    public GameObject[] Bullets;
    public static GameObject[] _Bullets;

    public GameObject[] EnemyBullets;
    public static GameObject[] _EnemyBullets;

    public GameObject[] PowerUps;
    public static GameObject[] _PowerUps;

    public GameObject[] Credits;
    public static GameObject[] _Credits;


    public GameObject[] PlayerShips;
    public static GameObject[] _PlayerShips;

    public Sprite[] TurretSprites;
    public static Sprite[] _TurretSprites;


    public GameObject[] Effects;
    public static GameObject[] _Effects;

    void Start() {
        
        _Bullets = new GameObject[Bullets.Length];
        _Bullets = Bullets;

        _EnemyBullets = new GameObject[EnemyBullets.Length];
        _EnemyBullets = EnemyBullets;

        _PowerUps = new GameObject[PowerUps.Length];
        _PowerUps = PowerUps;

        _Credits = new GameObject[Credits.Length];
        _Credits = Credits;

        _PlayerShips = new GameObject[PlayerShips.Length];
        _PlayerShips = PlayerShips;

        _TurretSprites = new Sprite[TurretSprites.Length];
        _TurretSprites = TurretSprites;


        _Effects = new GameObject[Effects.Length];
        _Effects = Effects;
        Debug.Log("Assinged all Static Object Arrays");
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

    public static int GetPowerUpIndex(PickUpBehaviourScript.PickUpTypes _PowerUpType) {
        int i = 0;
        foreach (GameObject go in _PowerUps) {
            if (go != null) {
                if (go.GetComponentInChildren<PickUpBehaviourScript>().thisPickUpType == _PowerUpType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _PowerUpType);
        return -1;
    }

    public static int GetCreditValueIndex(float _CreditValue) {
        int i = 0;
        foreach (GameObject go in _Credits) {
            if (go != null) {
                if (go.GetComponentInChildren<PickUpBehaviourScript>().CreditValue == _CreditValue) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not a Credit with the Value " + _CreditValue.ToString());
        return -1;
    }

    public static int GetPlayerShipIndex(PlayerControllerScript.Ships _PlayerShip) {
        int i = 0;
        foreach (GameObject go in _PlayerShips) {
            if (go != null) {
                if (go.GetComponentInChildren<PlayerControllerScript>().currendShip == _PlayerShip) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _PlayerShip);
        return -1;
    }

    public static int GetEffectIndex(EffectBehaviourScript.EffectTypes _effectType) {
        int i = 0;
        foreach (GameObject go in _Effects) {
            if (go != null) {
                if (go.GetComponentInChildren<EffectBehaviourScript>().effectType == _effectType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _effectType);
        return -1;
    }
}
