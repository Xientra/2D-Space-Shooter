using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {

    public bool doOnce = true;
    public bool showTutorials = true;

    public float currendCredits = 300f;
    public float HightScore = 0f;
    public bool[] LevelProgress = { false, false, false };

    public bool[] UnlockedWeapons;
    public bool UnlockedlvlXWeapons = false;

    public int playerFirstWeaponInt;
    public int playerSecondWeaponInt;

    public float NewWeaponPrice = 200f;

}