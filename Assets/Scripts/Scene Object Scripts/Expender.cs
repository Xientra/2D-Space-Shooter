using UnityEngine;

public class Expender : EnemyBehaviourScript {

    
    public enum ExpenderShips {
        ExpenderSpaceShip, ExpenderSpaceShipWithTurret, ExpenderMidSpaceShip, ExpenderBigSpaceShip, ExpenderSmallSpaceShip, ExpenderTransporterSpaceShip
    }
    public ExpenderShips ExpenderType = ExpenderShips.ExpenderSpaceShip;

    public enum ExpenderWeapons {
        None, ExpendersBullet
    }
    public ExpenderWeapons ExpenderWeapon = ExpenderWeapons.None;
    


}