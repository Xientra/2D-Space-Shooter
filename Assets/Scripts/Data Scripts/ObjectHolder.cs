using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour {

    public static ObjectHolder instance = null;


    public GameObject[] Bullets;
    public static GameObject[] _Bullets;

    public GameObject[] PlayerWeapons;
    public static GameObject[] _PlayerWeapons;

    public GameObject[] PowerUps;
    public static GameObject[] _PowerUps;

    public GameObject[] Credits;
    public static GameObject[] _Credits;

    public GameObject[] PlayerShips;
    public static GameObject[] _PlayerShips;

    public GameObject[] Turrets;
    public static GameObject[] _Turrets;

    public GameObject[] Effects;
    public static GameObject[] _Effects;

    private bool GameJustStarted = true;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("There is more than one ObjectHolder in the scene");
        }

        if (GameJustStarted == true) {
            _Bullets = new GameObject[Bullets.Length];
            _Bullets = Bullets;

            _PlayerWeapons = new GameObject[PlayerWeapons.Length];
            _PlayerWeapons = PlayerWeapons;

            _PowerUps = new GameObject[PowerUps.Length];
            _PowerUps = PowerUps;

            _Credits = new GameObject[Credits.Length];
            _Credits = Credits;

            _PlayerShips = new GameObject[PlayerShips.Length];
            _PlayerShips = PlayerShips;

            _Turrets = new GameObject[Turrets.Length];
            _Turrets = Turrets;

            _Effects = new GameObject[Effects.Length];
            _Effects = Effects;

            Debug.Log("Assinged all Static Object Arrays in Awake of ObjectHolderGo");
            GameJustStarted = false;
        }
    }

    public static int GetBulletIndex(LaserBulletBehaviourScript.BulletTypes _bulletType) {
        int i = 0;
        foreach (GameObject go in _Bullets) {
            if (go != null) {
                if (go.GetComponentInChildren<LaserBulletBehaviourScript>().bulletType == _bulletType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _bulletType);
        return -1;
    }

    public static int GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes _weaponType) {
        int i = 0;
        foreach (GameObject go in _PlayerWeapons) {
            if (go != null) {
                if (go.GetComponentInChildren<WeaponBehaviourScript>().WeaponType == _weaponType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _weaponType);
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

    public static int GetPlayerShipIndex(PlayerBehaviourScript.Ships _PlayerShip) {
        int i = 0;
        foreach (GameObject go in _PlayerShips) {
            if (go != null) {
                if (go.GetComponentInChildren<PlayerBehaviourScript>().currendShip == _PlayerShip) {
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

    /*
    public static int GetTurretIndex(PlayerControllerScript.Weapons _weapon) {
        int i = 0;
        string weaponName = null;
        switch (_weapon) {
            case (PlayerControllerScript.Weapons.Standart_lvl_1):
                weaponName = "StandartTurret_lvl_1";
                break;
            case (PlayerControllerScript.Weapons.Standart_lvl_2):
                weaponName = "StandartTurret_lvl_2";
                break;
            case (PlayerControllerScript.Weapons.Standart_lvl_3):
                weaponName = "StandartTurret_lvl_3";
                break;
            case (PlayerControllerScript.Weapons.LaserSword_lvl_1):
                weaponName = "LaserSwordTurret_lvl_1";
                break;
            case (PlayerControllerScript.Weapons.GrenadeLauncher_lvl_1):
                weaponName = "GrenadeLauncherTurret_lvl_1";
                break;
        }
        if (weaponName != null) {
            foreach (GameObject Go in _Turrets) {
                if (Go != null) {
                    if (Go.name == weaponName) {
                        return i;
                    }
                    i++;
                }
            }
            Debug.LogError("Could not find: " + _weapon);
        }
        else Debug.LogWarning("There is not Sprite to this Weaopn assinged");
        return 0; //Just the default sprite
    }
    */

    public static int GetWeaponTurretIndex(WeaponBehaviourScript.WeaponTypes _weaponType) {
        int i = 0;
        string weaponName = null;
        switch (_weaponType) {
            case (WeaponBehaviourScript.WeaponTypes.Standart_lvl_1):
                weaponName = "StandartTurret_lvl_1";
                break;
            case (WeaponBehaviourScript.WeaponTypes.Standart_lvl_2):
                weaponName = "StandartTurret_lvl_2";
                break;
            case (WeaponBehaviourScript.WeaponTypes.Standart_lvl_3):
                weaponName = "StandartTurret_lvl_3";
                break;
            case (WeaponBehaviourScript.WeaponTypes.LaserSword_lvl_1):
                weaponName = "LaserSwordTurret_lvl_1";
                break;
            case (WeaponBehaviourScript.WeaponTypes.LaserSword_lvl_2):
            case (WeaponBehaviourScript.WeaponTypes.LaserSword_lvl_3):
                weaponName = "LaserSwordTurret_lvl_1";
                break;
            case (WeaponBehaviourScript.WeaponTypes.GrenadeLauncher_lvl_1):
                weaponName = "GrenadeLauncherTurret_lvl_1";
                break;
        }
        if (weaponName != null) {
            foreach (GameObject Go in _Turrets) {
                if (Go != null) {
                    if (Go.name == weaponName) {
                        return i;
                    }
                    i++;
                }
            }
            Debug.LogError("Could not find: " + _weaponType);
        }
        else Debug.LogWarning("There is no Sprite, assinged to this weapon");
        return 0; //Just the default sprite
    }
}