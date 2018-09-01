using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControllerScript : MonoBehaviour {

    public GameObject StartMenu;
    //public GameObject StoryMenu;
    public GameObject EndlessLevelMenu;
    public GameObject ShopMenu;
    //public GameObject OptionsMenu;

    public static PlayerControllerScript.Weapons firstWeapon = PlayerControllerScript.Weapons.Standart_lvl_1;
    public static PlayerControllerScript.Weapons secondWeapon = PlayerControllerScript.Weapons.Helix_lvl_1;

    public bool AllWeaponsUnlocked = false;

    private PlayerControllerScript.Weapons SelectedWeapon;
    private int SelectedWeaponNumber = -1;
    private bool[] BoughtWeapons;

	void Start () {
        BoughtWeapons = new bool[PlayerControllerScript.Weapons.GetNames(typeof(PlayerControllerScript.Weapons)).Length];
        for (int i = 0; i < BoughtWeapons.Length; i++){
            BoughtWeapons[i] = AllWeaponsUnlocked;
        }

        UpdateUI();
    }
	
	void Update () {
		
	}

    private void UpdateUI() {
        GameObject.FindGameObjectsWithTag("Currend Credits UI")[0].GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
        GameObject.FindGameObjectsWithTag("First Weapon UI")[0].GetComponent<Text>().text = "Weapon:" + System.Environment.NewLine + firstWeapon.ToString();
        GameObject.FindGameObjectsWithTag("Second Weapon UI")[0].GetComponent<Text>().text = "Weapon:" + System.Environment.NewLine + secondWeapon.ToString();
    }

    public void Story_Btn() {
        Debug.Log("Mayby later");
    }

    public void Endless_Btn() {
        SceneManager.LoadScene("Endless Level");
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
                firstWeapon = (PlayerControllerScript.Weapons)SelectedWeaponNumber;
            }
            else {
                Debug.Log("You'll have to buy this Weapon first");
            }
        }
        else {
            Debug.Log("Please Select a Weapon"); //Please make this acually visible
        }
        UpdateUI();
    }

    public void SelectAsSecond() {
        if (SelectedWeaponNumber != -1) {
            if (BoughtWeapons[SelectedWeaponNumber] == true) {
                secondWeapon = (PlayerControllerScript.Weapons)SelectedWeaponNumber;
            }
            else {
                Debug.Log("You'll have to buy this Weapon first");
            }
        }
        else {
            Debug.Log("Please Select a Weapon"); //Please make this acually visible
        }
        UpdateUI();
    }

    public void Buy() {

    }

    public void SelectStandart_lvl_1() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Standart_lvl_1;
        SelectedWeaponNumber = (int)PlayerControllerScript.Weapons.Standart_lvl_1;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
    }
    public void SelectStandart_lvl_2() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Standart_lvl_2;
        SelectedWeaponNumber = (int)PlayerControllerScript.Weapons.Standart_lvl_2;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
    }
    public void SelectStandart_lvl_3() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Standart_lvl_3;
        SelectedWeaponNumber = (int)PlayerControllerScript.Weapons.Standart_lvl_3;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
    }
    public void SelectHelix_lvl_1() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Helix_lvl_1;
        SelectedWeaponNumber = (int)PlayerControllerScript.Weapons.Helix_lvl_1;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
    }
    public void SelectHelix_lvl_2() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Helix_lvl_2;
        SelectedWeaponNumber = (int)PlayerControllerScript.Weapons.Helix_lvl_2;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
    }
    public void SelectHelix_lvl_3() {
        //SelectedWeapon = PlayerControllerScript.Weapons.Helix_lvl_3;
        SelectedWeaponNumber = (int)PlayerControllerScript.Weapons.Helix_lvl_3;

        Debug.Log("Selected: " + SelectedWeapon); //Visual Indicator
    }

}
