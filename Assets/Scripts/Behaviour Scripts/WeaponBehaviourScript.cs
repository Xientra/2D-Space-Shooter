using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviourScript : MonoBehaviour {

    public enum WeaponTypes {
        Standart_lvl_1, Standart_lvl_2, Standart_lvl_3, Spread_lvl_1, Spread_lvl_2, Spread_lvl_3, Homing_lvl_1, Homing_lvl_2, Homing_lvl_3, Helix_lvl_1, Helix_lvl_2, Helix_lvl_3, ChainGun_lvl_1, ChainGun_lvl_2, ChainGun_lvl_3,
        WaveEmitter_lvl_1, LaserSword_lvl_1,
        RocketLauncher_lvl_1, GrenadeLauncher_lvl_1, ShrapnelLauncher_lvl_1, ShrapnelLauncher_lvl_2, ShrapnelLauncher_lvl_3,
        LaserGun, SplitLaserGun
    }

    public float cooldown;
    public WeaponTypes WeaponType;

    private float chainGunOffset = 0;
    private bool chainGunOffsetUp = true;
    float BulletRng;

    /*------------Stats for the Stats Menu--------------*/
    public bool isBought = false;
    public float price = 100;
    public string weaponName = "";
    public string description = "";

    void Start () {
		
	}
	
	void Update () {
		
	}


    public void Fire(Vector3 ProjectileSpawnPoint, GameObject TurretGameObject) {
        switch (WeaponType) {
            case (WeaponTypes.Standart_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Standart_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * 0.1f), TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * -0.1f), TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Standart_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * 0.2f), TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint + (TurretGameObject.transform.right * -0.2f), TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.Helix_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HelixBullet_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Helix_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HelixBullet_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.Helix_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HelixBullet_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.Spread_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -5f));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 5f));
                break;
            case (WeaponTypes.Spread_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -5));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 5));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -10));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 10));
                break;
            case (WeaponTypes.Spread_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -5f));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 5));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -10f));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 10f));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, -15));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Standart)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, 15));
                break;

            case (WeaponTypes.Homing_lvl_1):
                BulletRng = 1f;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HomingBullet_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                break;
            case (WeaponTypes.Homing_lvl_2):
                BulletRng = 6f;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HomingBullet_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                break;
            case (WeaponTypes.Homing_lvl_3):
                BulletRng = 12f;
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.HomingBullet_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(BulletRng, -BulletRng)));
                break;

            case (WeaponTypes.LaserSword_lvl_1):
                float fieldSize = 1.25f;
                Vector3 RndFieldPos = transform.position + (TurretGameObject.transform.right * Random.Range(-fieldSize, fieldSize)) + (TurretGameObject.transform.up * Random.Range(-fieldSize, -fieldSize / 2));//new Vector3(Random.Range(-fieldSize, fieldSize), ;
                Quaternion LookToMouse = Quaternion.LookRotation(Vector3.forward, (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - RndFieldPos));
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.LaserSword_lvl_1)], RndFieldPos, LookToMouse);
                break;

            case (WeaponTypes.WaveEmitter_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Wave)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.RocketLauncher_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Rocket)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.GrenadeLauncher_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Grenade)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.ShrapnelLauncher_lvl_1):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Shrapnel_lvl_1)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.ShrapnelLauncher_lvl_2):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Shrapnel_lvl_2)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;
            case (WeaponTypes.ShrapnelLauncher_lvl_3):
                Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.Shrapnel_lvl_3)], ProjectileSpawnPoint, TurretGameObject.transform.rotation);
                break;

            case (WeaponTypes.ChainGun_lvl_1):
                fireChainGun(0.04f, 0.1f, 6f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.ChainGun_lvl_2):
                fireChainGun(0.04f, 0.1f, 3f, ProjectileSpawnPoint, TurretGameObject);
                break;
            case (WeaponTypes.ChainGun_lvl_3):
                fireChainGun(0.04f, 0.1f, 1f, ProjectileSpawnPoint, TurretGameObject);
                break;
            /*
        case (WeaponTypes.LaserGun):
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
                            Instantiate(Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.SimpleLaser)], transform.position, transform.rotation, this.transform);
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
        case (WeaponTypes.SplitLaserGun):
            cooldown = 2f * fireRateMultiplyer;
            loadTime = 1f;
            fireAnyLaserGun(LaserBulletData.BulletTypes.SplitLaser);
            break;
            */
            default:
                Debug.LogError("The Weapon Type -" + WeaponType.ToString() + "- has no values assinged!");
                break;
        }

    }

    private void fireChainGun(float offsetSpeed, float maxOffset, float _BulletRng, Vector3 _ProjectileSpawnPoint, GameObject _TurretGameObject) {
        Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(LaserBulletData.BulletTypes.ChainGunBullet)], _ProjectileSpawnPoint + (_TurretGameObject.transform.right * chainGunOffset), _TurretGameObject.transform.rotation * Quaternion.Euler(0, 0, Random.Range(_BulletRng, -_BulletRng)));
        if (chainGunOffsetUp) chainGunOffset += offsetSpeed;
        else chainGunOffset -= offsetSpeed;
        if (chainGunOffset >= maxOffset) chainGunOffsetUp = false;
        if (chainGunOffset <= -maxOffset) chainGunOffsetUp = true;
    }
    /*
    private void fireAnyLaserGun(LaserBulletData.BulletTypes LaserToFire) {
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
