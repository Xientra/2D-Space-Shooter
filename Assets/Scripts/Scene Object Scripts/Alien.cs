using UnityEngine;

public class Alien : EnemyBehaviourScript {

    public enum AlienShips {
        AlienStandart, AlienTurret, AlienHeavy, AilenWingShip_straight, AilenWingShip_aim, AlienMiddleShip
    }
    public AlienShips AlienShip = AlienShips.AlienStandart;

    public enum AlienWeapons {
        None, FastSmall_aim, FiveSpreadSlow_straight, FourSmallLaserBullets_straight, OneBigForceFieldBullet_straight, OneBigForceFieldBullet_aim, OneSmallLaserBullets_straight
    }
    public AlienWeapons AlienWeapon = AlienWeapons.None;

}