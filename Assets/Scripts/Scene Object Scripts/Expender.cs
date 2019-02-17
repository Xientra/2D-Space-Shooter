using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expender : EnemyBehaviourScript {

    
    public new enum EnemyTypes {
        ExpenderSpaceShip, ExpenderSpaceShipWithTurret, ExpenderMidSpaceShip, ExpenderBigSpaceShip, ExpenderSmallSpaceShip, ExpenderTransporterSpaceShip
    }
    public EnemyTypes EnemyType = EnemyTypes.ExpenderSpaceShip;

    public new enum EnemyWeapons {
        None, ExpendersBullet
    }
    //public new EnemyWeapons EnemyWeapon = EnemyWeapons.None;
    


}