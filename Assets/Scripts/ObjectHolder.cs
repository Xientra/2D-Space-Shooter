using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour {

    public GameObject[] Bullets;
    static GameObject[] _Bullets;

    public GameObject[] EnemyBullets;
    static GameObject[] _EnemyBullets;


    void Start() {
        _Bullets = new GameObject[Bullets.Length];
        _Bullets = Bullets;

        _EnemyBullets = new GameObject[EnemyBullets.Length];
        _EnemyBullets = EnemyBullets;
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
}
