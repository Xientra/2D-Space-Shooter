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

    //Outfitter Elements
    private GameObject FirstWeaponDropdownGO;
    private GameObject SecondWeaponDropdownGO;
    private GameObject WeaponsViewGO;

    public static WeaponBehaviourScript.WeaponTypes firstWeapon = WeaponBehaviourScript.WeaponTypes.Standart_lvl_1;
    public static WeaponBehaviourScript.WeaponTypes secondWeapon = WeaponBehaviourScript.WeaponTypes.Helix_lvl_1;

    public static GameObject firstWeaponGO;
    public static GameObject secondWeaponGO;

    private PlayerControllerScript.Weapons SelectedWeapon;
    private int SelectedWeaponNumber = -1;

	void Start () {

        foreach (Transform t in OutfitterMenu.transform) {
            if (t.name == "First Weapon Dropdown") {
                FirstWeaponDropdownGO = t.gameObject;
            }
            if (t.name == "Second Weapon Dropdown") {
                SecondWeaponDropdownGO = t.gameObject;
            }
            if (t.name == "Weapons View") {
                WeaponsViewGO = t.gameObject;
            }
        }

        UpdateUI();
        StartCoroutine(DoStuffAfterOneFrame());
    }
    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;
        firstWeaponGO = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Standart_lvl_1)];
        secondWeaponGO = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1)];
    }

    void Update () {
		
	}

    private void UpdateUI() {
        if (OutfitterMenu.activeSelf == true) {
            GameObject.FindGameObjectsWithTag("Currend Credits UI")[0].GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
            //GameObject.FindGameObjectsWithTag("First Weapon UI")[0].GetComponent<Text>().text = "First Weapon:" + System.Environment.NewLine + firstWeapon.ToString();
            //GameObject.FindGameObjectsWithTag("Second Weapon UI")[0].GetComponent<Text>().text = "Second Weapon:" + System.Environment.NewLine + secondWeapon.ToString();
            
            
            
            if (FirstWeaponDropdownGO != null) {
                FirstWeaponDropdownGO.GetComponent<Dropdown>().options.Clear();
                foreach (GameObject pWep in ObjectHolder._PlayerWeapons)
                    if (pWep != null) {
                        if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true)
                            FirstWeaponDropdownGO.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pWep.GetComponent<WeaponBehaviourScript>().weaponName));
                    }
                    else Debug.LogWarning("There is a free element in the static _PlayerWeapons Array");
                FirstWeaponDropdownGO.GetComponentInChildren<Text>().text = FirstWeaponDropdownGO.GetComponent<Dropdown>().options[0].text;
            }
            else Debug.LogError("The FirstWeaponDropdownGO is null.");


            if (SecondWeaponDropdownGO != null) {
                SecondWeaponDropdownGO.GetComponent<Dropdown>().options.Clear();
                foreach (GameObject pWep in ObjectHolder._PlayerWeapons)
                    if (pWep != null) {
                        if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true)
                            SecondWeaponDropdownGO.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pWep.GetComponent<WeaponBehaviourScript>().weaponName));
                    }
                    else Debug.LogWarning("There is a free element in the static _PlayerWeapons Array");
                SecondWeaponDropdownGO.GetComponentInChildren<Text>().text = SecondWeaponDropdownGO.GetComponent<Dropdown>().options[0].text;
            }
            else Debug.LogError("The SecondWeaponDropdownGO is null.");
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
            if (t.name == "Weapon Level Text") t.GetComponent<Text>().text = "Level: " + ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().WeaponLevel.ToString().Remove(0, 1);
            if (t.name == "Weapon Price Text") t.GetComponent<Text>().text = "Price: " + ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().price.ToString();
            //if (t.name == "Weapon Damage Text") t.GetComponent<Text>().text = "Damage: " + ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().DamagePerShoot.ToString();
            if (t.name == "Weapon FireRate Text") t.GetComponent<Text>().text = "Fire Rate: " + ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(wT)].GetComponent<WeaponBehaviourScript>().cooldown.ToString();
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
        Debug.Log("Quit is not implemented");
    }

    public void SelectAsFirst() {
        if (SelectedWeaponNumber != -1) {
            firstWeapon = (WeaponBehaviourScript.WeaponTypes)SelectedWeaponNumber;
            //Debug.Log("You'll have to buy this Weapon first");
        }
        else {
            Debug.Log("Please Select a Weapon"); //Please make this acually visible
        }
        UpdateUI();

        WeaponInfoSrceen.SetActive(false);
    }

    public void SelectAsSecond() {
        if (SelectedWeaponNumber != -1) {
            secondWeapon = (WeaponBehaviourScript.WeaponTypes)SelectedWeaponNumber;
            //Debug.Log("You'll have to buy this Weapon first");
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

    public void OnValueChangeFirstWeaponDropdown() {
        int i = FirstWeaponDropdownGO.GetComponent<Dropdown>().value;
        int counter = 0;

        foreach (GameObject pWep in ObjectHolder._PlayerWeapons) {
            if (pWep != null)
                if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true) {
                    if (counter == i)
                        firstWeaponGO = pWep;
                    counter++;
                }
        }

        Debug.Log("first weapon = "+firstWeaponGO.name);
    }

    public void OnValueChangeSecondWeaponDropdown() {
        int i = SecondWeaponDropdownGO.GetComponent<Dropdown>().value;
        int counter = 0;

        foreach (GameObject pWep in ObjectHolder._PlayerWeapons) {
            if (pWep != null)
                if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true) {
                    if (counter == i)
                        secondWeaponGO = pWep;
                    counter++;
                }
        }

        Debug.Log("second weapon = "+secondWeaponGO.name);
    }

    //Is called from WeaponsViewElementDataScript
    public void OpenWeaponInfoScreen_Btn(GameObject _weaponObject) {
        if (_weaponObject.GetComponent<WeaponBehaviourScript>() != null) {
            StartMenu.SetActive(false);
            //StoryMenu.SetActive(false);
            //EndlessLevelMenu.SetActive(false);
            OutfitterMenu.SetActive(true);
            WeaponInfoSrceen.SetActive(true);
            //OptionsMenu.SetActive(false);

            foreach (Transform t in WeaponInfoSrceen.transform) {
                if (t.name == "Weapon Name Text") t.GetComponent<Text>().text = _weaponObject.GetComponent<WeaponBehaviourScript>().weaponName;
                if (t.name == "Weapon Level Text") t.GetComponent<Text>().text = "Level: " + _weaponObject.GetComponent<WeaponBehaviourScript>().WeaponLevel.ToString().Remove(0, 1);
                if (t.name == "Weapon Price Text") t.GetComponent<Text>().text = "Price: " + _weaponObject.GetComponent<WeaponBehaviourScript>().price.ToString();
                //if (t.name == "Weapon Damage Text") t.GetComponent<Text>().text = "Damage: " + _weaponObject.GetComponent<WeaponBehaviourScript>().DamagePerShoot.ToString();
                if (t.name == "Weapon FireRate Text") t.GetComponent<Text>().text = "Fire Rate: " + _weaponObject.GetComponent<WeaponBehaviourScript>().cooldown.ToString();
                if (t.name == "Weapon Description Text") t.GetComponent<Text>().text = _weaponObject.GetComponent<WeaponBehaviourScript>().description;
            }

        }
        else {
            Debug.LogError("OpenWeaponInfoScreen_Btn hass been called with the object " + _weaponObject.name + ", which has no WeaponBehaviourScript ssinged.");
        }
    }
}