using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControllerScript : MonoBehaviour {

    public GameObject StartMenu;
    //public GameObject StoryMenu;
    //public GameObject EndlessLevelMenu;
    public GameObject OutfitterMenu;
    public GameObject WeaponInfoSrceen;
    //public GameObject OptionsMenu;

    public static WeaponBehaviourScript.WeaponTypes firstWeapon = WeaponBehaviourScript.WeaponTypes.Standart_lvl_1;
    public static WeaponBehaviourScript.WeaponTypes secondWeapon = WeaponBehaviourScript.WeaponTypes.Helix_lvl_1;

    public bool AllWeaponsUnlocked = false;

    private PlayerControllerScript.Weapons SelectedWeapon;
    private int SelectedWeaponNumber = -1;
    private bool[] BoughtWeapons;

	void Start () {
        BoughtWeapons = new bool[WeaponBehaviourScript.WeaponTypes.GetNames(typeof(WeaponBehaviourScript.WeaponTypes)).Length];
        for (int i = 0; i < BoughtWeapons.Length; i++){
            BoughtWeapons[i] = AllWeaponsUnlocked;
        }

        UpdateUI();
    }
	
	void Update () {
		
	}

    private void UpdateUI() {
        if (OutfitterMenu.activeSelf == true) {
            GameObject.FindGameObjectsWithTag("Currend Credits UI")[0].GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
            GameObject.FindGameObjectsWithTag("First Weapon UI")[0].GetComponent<Text>().text = "Weapon:" + System.Environment.NewLine + firstWeapon.ToString();
            GameObject.FindGameObjectsWithTag("Second Weapon UI")[0].GetComponent<Text>().text = "Weapon:" + System.Environment.NewLine + secondWeapon.ToString();
        }
    }

    private void OpenWeaponInfoSrceen() {
        StartMenu.SetActive(false);
        //StoryMenu.SetActive(false);
        //EndlessLevelMenu.SetActive(false);
        OutfitterMenu.SetActive(true);
        WeaponInfoSrceen.SetActive(true);
        //OptionsMenu.SetActive(false);

        WeaponBehaviourScript.WeaponTypes wT = (WeaponBehaviourScript.WeaponTypes)SelectedWeaponNumber;
        foreach (Transform t in WeaponInfoSrceen.transform) {
            if (t.name == "Weapon Name Text") t.GetComponent<Text>().text = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().weaponName;
            if (t.name == "Weapon Price Text") t.GetComponent<Text>().text = "Price: " + ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().price.ToString();
            if (t.name == "Weapon Description Text") t.GetComponent<Text>().text = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().description;
        }
    }


    /*-------------------------------------------UI Buttons, etc.-------------------------------------------------------*/
    public void Story_Btn() {
        Debug.Log("Mayby later");
    }

    public void Endless_Btn() {
        SceneManager.LoadScene("Endless Level");
    }

    public void Outfitter_Btn() {
        StartMenu.SetActive(false);
        //StoryMenu.SetActive(false);
        //EndlessLevelMenu.SetActive(false);
        OutfitterMenu.SetActive(true);
        WeaponInfoSrceen.SetActive(false);
        //OptionsMenu.SetActive(false);

    }

    public void Options_Btn() {
        Debug.Log("lol no");
    }

    public void Quit_Btn() {
        Debug.Log("Quit");
    }

    public void SelectAsFirst() {
        if (SelectedWeaponNumber != -1) {
            if (BoughtWeapons[SelectedWeaponNumber] == true) {
                firstWeapon = (WeaponBehaviourScript.WeaponTypes)SelectedWeaponNumber;
            }
            else {
                Debug.Log("You'll have to buy this Weapon first");
            }
        }
        else {
            Debug.Log("Please Select a Weapon"); //Please make this acually visible
        }
        UpdateUI();

        WeaponInfoSrceen.SetActive(false);
    }

    public void SelectAsSecond() {
        if (SelectedWeaponNumber != -1) {
            if (BoughtWeapons[SelectedWeaponNumber] == true) {
                secondWeapon = (WeaponBehaviourScript.WeaponTypes)SelectedWeaponNumber;
            }
            else {
                Debug.Log("You'll have to buy this Weapon first");
            }
        }
        else {
            Debug.Log("Please Select a Weapon"); //Please make this acually visible
        }
        UpdateUI();

        WeaponInfoSrceen.SetActive(false);
    }

    public void Btn_Buy() {

    }

    public void Btn_WeaponSelectScreenBack() {
        SelectedWeaponNumber = - 1;
        WeaponInfoSrceen.SetActive(false);
    }

    public void Btn_Back() {
        StartMenu.SetActive(true);
        //StoryMenu.SetActive(false);
        //EndlessLevelMenu.SetActive(false);
        OutfitterMenu.SetActive(false);
        //OptionsMenu.SetActive(false);
    }

    public void SelectStandart_lvl_1() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Standart_lvl_1;
        SelectedWeaponNumber = (int)WeaponBehaviourScript.WeaponTypes.Standart_lvl_1;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
        OpenWeaponInfoSrceen();
    }
    public void SelectStandart_lvl_2() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Standart_lvl_2;
        SelectedWeaponNumber = (int)WeaponBehaviourScript.WeaponTypes.Standart_lvl_2;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
        OpenWeaponInfoSrceen();
    }
    public void SelectStandart_lvl_3() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Standart_lvl_3;
        SelectedWeaponNumber = (int)WeaponBehaviourScript.WeaponTypes.Standart_lvl_3;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
        OpenWeaponInfoSrceen();
    }
    public void SelectHelix_lvl_1() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Helix_lvl_1;
        SelectedWeaponNumber = (int)WeaponBehaviourScript.WeaponTypes.Helix_lvl_1;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
        OpenWeaponInfoSrceen();
    }
    public void SelectHelix_lvl_2() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Helix_lvl_2;
        SelectedWeaponNumber = (int)WeaponBehaviourScript.WeaponTypes.Helix_lvl_2;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
        OpenWeaponInfoSrceen();
    }
    public void SelectHelix_lvl_3() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Helix_lvl_3;
        SelectedWeaponNumber = (int)WeaponBehaviourScript.WeaponTypes.Helix_lvl_3;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
        OpenWeaponInfoSrceen();
    }

}
