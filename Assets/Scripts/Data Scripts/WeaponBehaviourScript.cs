﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviourScript : MonoBehaviour {

    public enum WeaponTypes {
        Standart_lvl_1, Standart_lvl_2, Standart_lvl_3, Shotgun_lvl_1, Shotgun_lvl_2, Shotgun_lvl_3, Homing_lvl_1, Homing_lvl_2, Homing_lvl_3, Helix_lvl_1, Helix_lvl_2, Helix_lvl_3,
        ChainGun_lvl_1, ChainGun_lvl_2, ChainGun_lvl_3, WaveEmitter_lvl_1, WaveEmitter_lvl_2, WaveEmitter_lvl_3, LaserSword_lvl_1, LaserSword_lvl_2, LaserSword_lvl_3,
        MissileLauncher_lvl_1, MissileLauncher_lvl_2, MissileLauncher_lvl_3, GrenadeLauncher_lvl_1, GrenadeLauncher_lvl_2, GrenadeLauncher_lvl_3, ShrapnelLauncher_lvl_1, ShrapnelLauncher_lvl_2, ShrapnelLauncher_lvl_3,
    }

    public float cooldown;
    public WeaponTypes WeaponType;

    //vars for the different firering types of the weapons
    private float chainGunOffset = 0;
    private bool chainGunOffsetUp = true;
    float BulletRng;
    private bool Standartlvl3OffsetDirection = true;


    public GameObject TurretGameObject;

    [Header("UI Values")]

    /*------------Stats for the Stats Menu--------------*/
    public bool isBought = false;
    public enum WeaponLevels { _1, _2, _3}
    public WeaponLevels WeaponLevel = WeaponLevels._1;
    public float price = 100;
    public string weaponName = "";
    public string description = "";
    public Sprite WeaponImage;
    //public float DamagePerShoot = 20;

    public GameObject PreviousWeapon;
    public GameObject NextWeapon;

    void Start() {
    }

    void Update() {

    }

    //UnityEditor.EditorApplication.isPaused = true;
    public void Fire(Vector3 ProjectileSpawnPoint, GameObject TurretGameObject) {
        switch (WeaponType) {
            case (WeaponTypes.Standart_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Standart_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * 0.1f), TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * -0.1f), TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Standart_lvl_3):
                float _value = 0.1125f;
                float _offset = _value;
                if (Standartlvl3OffsetDirection == true)
                    _offset = _value;
                if (Standartlvl3OffsetDirection == false)
                    _offset = -_value;
                Standartlvl3OffsetDirection = !Standartlvl3OffsetDirection;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * _offset), TurretGameObject.transform.rotation);

                //Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                //Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * 0.2f), TurretGameObject.transform.rotation);
                //Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * -0.2f), TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.Helix_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Helix_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Helix_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.Shotgun_lvl_1):
                fireShotgun(7, 10f, 0.4f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.Shotgun_lvl_2):
                fireShotgun(11, 12f, 0.4f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.Shotgun_lvl_3):
                fireShotgun(15, 14f, 0.4f, ProjectileSpawnPoint, TurretGameObject);
                break;

            case (WeaponTypes.Homing_lvl_1):
                BulletRng = 1f;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                break;
            case (WeaponTypes.Homing_lvl_2):
                BulletRng = 6f;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                break;
            case (WeaponTypes.Homing_lvl_3):
                BulletRng = 12f;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                break;

            case (WeaponTypes.LaserSword_lvl_1):
                fireLaserSowrd(1.5f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.LaserSword_lvl_2):
                fireLaserSowrd(1.75f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.LaserSword_lvl_3):
                fireLaserSowrd(2f, ProjectileSpawnPoint, TurretGameObject);
                break;

            case (WeaponTypes.WaveEmitter_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Wave)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.MissileLauncher_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Missile_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.MissileLauncher_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Missile_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.MissileLauncher_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Missile_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.GrenadeLauncher_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Grenade_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.GrenadeLauncher_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Grenade_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.GrenadeLauncher_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Grenade_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.ShrapnelLauncher_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.ShrapnelLauncher_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.ShrapnelLauncher_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.ChainGun_lvl_1):
                fireChainGun(0.03f, 0.09f, 6f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.ChainGun_lvl_2):
                fireChainGun(0.03f, 0.09f, 3f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.ChainGun_lvl_3):
                fireChainGun(0.03f, 0.09f, 1f, ProjectileSpawnPoint, TurretGameObject);
                break;

            default:
                Debug.LogError("The Weapon Type -" + WeaponType.ToString() + "- has no values assinged!");
                break;
        }
    }

    private void fireChainGun(float offsetSpeed, float maxOffset, float _BulletRng, Vector3 _ProjectileSpawnPoint, GameObject _TurretGameObject) {
        Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.ChainGunBullet)], _ProjectileSpawnPoint + (_TurretGameObject.transform.right * chainGunOffset), _TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(_BulletRng, -_BulletRng)));
        if (chainGunOffsetUp) chainGunOffset += offsetSpeed;
        else chainGunOffset -= offsetSpeed;
        if (chainGunOffset >= maxOffset) chainGunOffsetUp = false;
        if (chainGunOffset <= -maxOffset) chainGunOffsetUp = true;
    }

    private void fireLaserSowrd(float _fieldSize, Vector3 _ProjectileSpawnPoint, GameObject _TurretGameObject) {
        Vector3 RndFieldPos = _ProjectileSpawnPoint + (_TurretGameObject.transform.right * Random.Range(-_fieldSize, _fieldSize)) + (_TurretGameObject.transform.up * Random.Range(-_fieldSize, -_fieldSize / 2));
        Quaternion LookToMouse = Quaternion.LookRotation(Vector3.forward, (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - RndFieldPos));

        Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.LaserSword_lvl_1)], RndFieldPos, LookToMouse);
    }

    private void fireShotgun(int _bulletAmount, float _bulletRng, float _bulletDisplacement,  Vector3 _ProjectileSpawnPoint, GameObject _TurretGameObject) {
        for (int i = 1; i <= _bulletAmount; i++) {
            Vector3 displacement = (_TurretGameObject.transform.up * Random.Range(-_bulletDisplacement, _bulletDisplacement));
            Quaternion spread = Quaternion.Euler(0, 0, Random.Range(_bulletRng, -_bulletRng));

            Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.ShotgunBullet)], _ProjectileSpawnPoint + displacement, _TurretGameObject.transform.rotation * spread);
        }
        //UnityEditor.EditorApplication.isPaused = true;
    }
    /*
    private void fireAnyLaserGun(LaserBulletBehaviourScript.BulletTypes LaserToFire) {
        if (cooldownTimeStamp <= Time.time) {
            if (Input.GetButtonDown("Fire1")) {
                pressedButtonDown = true;
                loadTimeStamp = Time.time + loadTime;
                isReady = true;
            }
            if (loadTimeStamp >= Time.time) {
                if (isReady == true) {
                    Debug.Log("Loading");
                }
            }
            if (loadTimeStamp <= Time.time) {
                if (isReady == true) {
                    Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.LaserLoaded)], transform.position, transform.rotation, transform);
                    isReady = false;
                }
            }
            if (pressedButtonDown == true) {
                if (Input.GetButtonUp("Fire1")) {
                    if (loadTimeStamp <= Time.time) {
                        Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserToFire)], transform.position, transform.rotation, this.transform);
                        loadTimeStamp = Time.time + loadTime;
                        cooldownTimeStamp = Time.time + cooldown; //Apply cooldown when firered
                    }
                    else {
                        loadTimeStamp = Time.time + loadTime;
                        //no cooldown wehen reset
                        Debug.Log("reset");
                    }
                    isReady = false;
                    pressedButtonDown = false;
                }
            }
        }
    }
    */
}
/*
            switch (currentWeapon) {
                case (Weapons.WaveEmitter_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.3f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Wave)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.GrenadeLauncher_lvl_1):
                    if (Input.GetButton("Fire1")) {
                        cooldown = 0.8f * fireRateMultiplyer;
                        if (cooldownTimeStamp <= Time.time) {
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Grenade)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                            cooldownTimeStamp = Time.time + cooldown;
                        }
                    }
                    break;
                case (Weapons.LaserGun):
                    cooldown = 2f * fireRateMultiplyer;
                    loadTime = 1f;
                    if (cooldownTimeStamp <= Time.time) {
                        if (Input.GetButtonDown("Fire1")) {
                            pressedButtonDown = true;
                            loadTimeStamp = Time.time + loadTime;
                            isReady = true;
                        }
                        if (loadTimeStamp >= Time.time) {
                            if (isReady == true) {
                                Debug.Log("Loading");
                            }
                        }
                        if (loadTimeStamp <= Time.time) {
                            if (isReady == true) {
                                Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.LaserLoaded)], transform.position, transform.rotation, transform);
                                isReady = false;
                            }
                        }
                        if (pressedButtonDown == true) {
                            if (Input.GetButtonUp("Fire1")) {
                                if (loadTimeStamp <= Time.time) {
                                    Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.SimpleLaser)], transform.position, transform.rotation, this.transform);
                                    loadTimeStamp = Time.time + loadTime;
                                    cooldownTimeStamp = Time.time + cooldown; //Apply cooldown when firered
                                }
                                else {
                                    loadTimeStamp = Time.time + loadTime;
                                    //no cooldown wehen reset
                                    Debug.Log("reset");
                                }
                                isReady = false;
                                pressedButtonDown = false;
                            }
                        }
                    }
                    break;
                case (Weapons.SplitLaserGun):
                    cooldown = 2f * fireRateMultiplyer;
                    loadTime = 1f;
                    fireAnyLaserGun(LaserBulletBehaviourScript.BulletTypes.SplitLaser);
                    break;
                default:
                    Debug.LogError("The Weapon Type -" + currentWeapon.ToString() + "- has no values assinged!");
                    break;
            }
            */

/*
 * Vars for firering a laser
//laser firering vars
private float loadTime = 0.2f;
private float loadTimeStamp;
private bool isReady = false;
private bool pressedButtonDown = false;
*/
/*
switch (currentWeapon) {
case (Weapons.Standart_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.3f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Standart_lvl_2):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.3f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * 0.1f), TurretRotationAnchorGo.transform.rotation);
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * -0.1f), TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Standart_lvl_3):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.4f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * 0.2f), TurretRotationAnchorGo.transform.rotation);
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretRotationAnchorGo.transform.right * -0.2f), TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Helix_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.5f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_1)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Helix_lvl_2):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.7f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_2)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Helix_lvl_3):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.9f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HelixBullet_lvl_3)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Spread):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.4f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, -10));
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Standart)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, 10));
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Homing_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.3f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            float BulletRng = 1f;
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_1)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Homing_lvl_2):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.2f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            float BulletRng = 6f;
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_2)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.Homing_lvl_3):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.1f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            float BulletRng = 12f;
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.HomingBullet_lvl_3)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.LaserSword_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.5f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            float fieldSize = 1.25f;
            Vector3 RndFieldPos = transform.position + (TurretRotationAnchorGo.transform.right * Random.Range(-fieldSize, fieldSize)) + (TurretRotationAnchorGo.transform.up * Random.Range(-fieldSize, -fieldSize / 2));//new Vector3(Random.Range(-fieldSize, fieldSize), ;
            Quaternion LookToMouse = Quaternion.LookRotation(Vector3.forward, (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - RndFieldPos));
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.LaserSword_lvl_1)], RndFieldPos, LookToMouse);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.WaveEmitter_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.3f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Wave)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.RocketLauncher_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.8f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Rocket)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.GrenadeLauncher_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.8f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Grenade)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.ShrapnelLauncher_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.8f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_1)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.ShrapnelLauncher_lvl_2):
    if (Input.GetButton("Fire1")) {
        cooldown = 1.2f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_2)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.ShrapnelLauncher_lvl_3):
    if (Input.GetButton("Fire1")) {
        cooldown = 1.4f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.Shrapnel_lvl_3)], ProjectileSpawnPoint, TurretRotationAnchorGo.transform.rotation);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.ChainGun_lvl_1):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.01f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            fireChainGun(0.04f, 0.1f, 6f);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.ChainGun_lvl_2):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.01f * fireRateMultiplyer * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            fireChainGun(0.04f, 0.1f, 3f);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.ChainGun_lvl_3):
    if (Input.GetButton("Fire1")) {
        cooldown = 0.005f * fireRateMultiplyer;
        if (cooldownTimeStamp <= Time.time) {
            fireChainGun(0.04f, 0.1f, 1f);
            cooldownTimeStamp = Time.time + cooldown;
        }
    }
    break;
case (Weapons.LaserGun):
    cooldown = 2f * fireRateMultiplyer;
    loadTime = 1f;
    if (cooldownTimeStamp <= Time.time) {
        if (Input.GetButtonDown("Fire1")) {
            pressedButtonDown = true;
            loadTimeStamp = Time.time + loadTime;
            isReady = true;
        }
        if (loadTimeStamp >= Time.time) {
            if (isReady == true) {
                Debug.Log("Loading");
            }
        }
        if (loadTimeStamp <= Time.time) {
            if (isReady == true) {
                Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.LaserLoaded)], transform.position, transform.rotation, transform);
                isReady = false;
            }
        }
        if (pressedButtonDown == true) {
            if (Input.GetButtonUp("Fire1")) {
                if (loadTimeStamp <= Time.time) {
                    Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletBehaviourScript.BulletTypes.SimpleLaser)], transform.position, transform.rotation, this.transform);
                    loadTimeStamp = Time.time + loadTime;
                    cooldownTimeStamp = Time.time + cooldown; //Apply cooldown when firered
                }
                else {
                    loadTimeStamp = Time.time + loadTime;
                    //no cooldown wehen reset
                    Debug.Log("reset");
                }
                isReady = false;
                pressedButtonDown = false;
            }
        }
    }
    break;
case (Weapons.SplitLaserGun):
    cooldown = 2f * fireRateMultiplyer;
    loadTime = 1f;
    fireAnyLaserGun(LaserBulletBehaviourScript.BulletTypes.SplitLaser);
    break;
default:
    Debug.LogError("The Weapon Type -" + currentWeapon.ToString() + "- has no values assinged!");
    break;
}
*/