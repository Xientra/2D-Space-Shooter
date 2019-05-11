using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsViewElementDataScript : MonoBehaviour {

    public GameObject MainMenuControllerGo;

    public GameObject WeaponPreafab;

    public bool isBought;

    public void OpenWeaponInfoScreen_Btn() {
        //Debug.Log(this.gameObject.name);
        MainMenuControllerGo.GetComponent<MainMenuControllerScript>().OpenWeaponInfoScreen(WeaponPreafab);

        AudioControllerScript.activeInstance.PlaySound("ButtonPress");
    }
}