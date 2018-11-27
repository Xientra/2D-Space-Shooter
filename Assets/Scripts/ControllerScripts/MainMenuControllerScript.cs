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
    private GameObject WeaponInfoScreenWeaponGo = null;
    //for the movement of the back and the upgrade buttons
    private float BackButtonStandartPos = -1;


    public static GameObject firstWeaponGO;
    public static GameObject secondWeaponGO;

    public static float NewWeaponPrice = 200f;


    void Start () {

        //GameControllerScript.currendCredits += 10000;

        //Sets all Weapons.isBought to false exept for a few selected ones
        foreach (GameObject playerWeapon in ObjectHolder._PlayerWeapons) {
            switch (playerWeapon.GetComponent<WeaponBehaviourScript>().WeaponType) {
                case WeaponBehaviourScript.WeaponTypes.Standart_lvl_1:
                    playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = true;
                    break;
                case WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1:
                    playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = true;
                    break;

                default:
                    playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = false;
                    break;
            }
        }


        foreach (Transform t in OutfitterMenu.transform) {
            if (t.name == "First Weapon Dropdown") {
                FirstWeaponDropdownGO = t.gameObject;
            }
            if (t.name == "Second Weapon Dropdown") {
                SecondWeaponDropdownGO = t.gameObject;
            }
            if (t.name == "Weapons View") {  //GameObject.FindGameObjectWithTag("Weapons View UI").GetComponent<WeaponsViewControllerScript>();
                WeaponsViewGO = t.gameObject;
            }
        }

        //UpdateUI();
        StartCoroutine(DoStuffAfterOneFrame());
    }
    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;
        firstWeaponGO = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Standart_lvl_1)];
        secondWeaponGO = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1)];
        UpdateUI();
    }

    void Update () {
		
	}

    private void UpdateUI() {
        if (OutfitterMenu.activeSelf == true) {

            foreach (Transform t in OutfitterMenu.transform) {
                if (t.CompareTag("Currend Credits UI")) t.GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
                //GameObject.FindGameObjectsWithTag("First Weapon UI")[0].GetComponent<Text>().text = "First Weapon:" + System.Environment.NewLine + firstWeapon.ToString();
                //GameObject.FindGameObjectsWithTag("Second Weapon UI")[0].GetComponent<Text>().text = "Second Weapon:" + System.Environment.NewLine + secondWeapon.ToString();


                if (t.name == "Btn_BuyNewWeapon") t.GetComponentsInChildren<Transform>()[1].GetComponent<Text>().text = NewWeaponPrice.ToString();
            }

            if (FirstWeaponDropdownGO != null) {
                int counter1 = 0;
                FirstWeaponDropdownGO.GetComponent<Dropdown>().options.Clear();
                foreach (GameObject pWep in ObjectHolder._PlayerWeapons)
                    if (pWep != null) {
                        if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true) {
                            FirstWeaponDropdownGO.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pWep.GetComponent<WeaponBehaviourScript>().weaponName));
                            if (firstWeaponGO != null)
                                if (pWep.GetComponent<WeaponBehaviourScript>().WeaponType == firstWeaponGO.GetComponent<WeaponBehaviourScript>().WeaponType) {
                                    FirstWeaponDropdownGO.GetComponent<Dropdown>().value = counter1;
                                }
                            counter1++;
                        }
                    }
                    else Debug.LogWarning("There is a free element in the static _PlayerWeapons Array");
                FirstWeaponDropdownGO.GetComponent<Dropdown>().RefreshShownValue();
            }
            else Debug.LogError("The FirstWeaponDropdownGO is null.");


            if (SecondWeaponDropdownGO != null) {
                int counter2 = 0;
                SecondWeaponDropdownGO.GetComponent<Dropdown>().options.Clear();
                foreach (GameObject pWep in ObjectHolder._PlayerWeapons)
                    if (pWep != null) {
                        if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true) {
                            SecondWeaponDropdownGO.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pWep.GetComponent<WeaponBehaviourScript>().weaponName));
                            if (secondWeaponGO != null)
                                if (pWep.GetComponent<WeaponBehaviourScript>().WeaponType == secondWeaponGO.GetComponent<WeaponBehaviourScript>().WeaponType) {
                                    SecondWeaponDropdownGO.GetComponent<Dropdown>().value = counter2;
                                }
                            counter2++;
                        }
                    }
                    else Debug.LogWarning("There is a free element in the static _PlayerWeapons Array");
                SecondWeaponDropdownGO.GetComponent<Dropdown>().RefreshShownValue();
            }
            else Debug.LogError("The SecondWeaponDropdownGO is null.");
        }
    }

    /*-------------------------------------------Main Menu-------------------------------------------------------*/
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
        UnityEditor.EditorApplication.isPlaying = false;
    }


    /*-----------------------------------------Outfitter Menu-----------------------------------------------*/
    public void Btn_BuyNewWeapon() {
        List<GameObject> buyableWeapons = new List<GameObject>();
        foreach (GameObject WepGo in ObjectHolder._PlayerWeapons) {
            if (WepGo != null) 
                if (WepGo.GetComponent<WeaponBehaviourScript>().WeaponLevel == WeaponBehaviourScript.WeaponLevels._1 && WepGo.GetComponent<WeaponBehaviourScript>().isBought == false) 
                    buyableWeapons.Add(WepGo);
        }

        if (buyableWeapons.Count != 0) {
            if (GameControllerScript.currendCredits >= NewWeaponPrice) {
                
                GameControllerScript.currendCredits -= NewWeaponPrice;
                NewWeaponPrice += 100;

                GameObject randomWeponGo = buyableWeapons[Random.Range(0, buyableWeapons.Count)];
                randomWeponGo.GetComponent<WeaponBehaviourScript>().isBought = true;
                WeaponsViewGO.GetComponent<WeaponsViewControllerScript>().UpdateWeaponsView();
                UpdateUI();
                OpenWeaponInfoScreen(randomWeponGo);
            }
            else Debug.Log("You need some visual feedback that the player has not enouth money to buy a new weapoon...");
        }
        else
            foreach (Transform t in OutfitterMenu.transform) {
                if (t.name == "Btn_BuyNewWeapon") t.GetComponent<Button>().interactable = false;
            }
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

    public void Btn_Back() {
        StartMenu.SetActive(true);
        //StoryMenu.SetActive(false);
        //EndlessLevelMenu.SetActive(false);
        OutfitterMenu.SetActive(false);
        //OptionsMenu.SetActive(false);
    }

    /*-----------------------------------------Weapon Info Screen-----------------------------------------------*/
    //Is called from WeaponsViewElementDataScript
    public void OpenWeaponInfoScreen(GameObject _weaponObject) {
        if (_weaponObject.GetComponent<WeaponBehaviourScript>() != null) {
            StartMenu.SetActive(false);
            //StoryMenu.SetActive(false);
            //EndlessLevelMenu.SetActive(false);
            OutfitterMenu.SetActive(true);
            WeaponInfoSrceen.SetActive(true);
            //OptionsMenu.SetActive(false);

            WeaponInfoScreenWeaponGo = _weaponObject;

            foreach (Transform t in WeaponInfoSrceen.transform) {
                if (t.name == "Weapon Name Text") t.GetComponent<Text>().text = WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().weaponName;
                if (t.name == "Weapon Level Text") t.GetComponent<Text>().text = "Level: " + WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().WeaponLevel.ToString().Remove(0, 1);
                if (t.name == "Weapon Price Text") t.GetComponent<Text>().text = "Price: " + WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().price.ToString();
                //if (t.name == "Weapon Damage Text") t.GetComponent<Text>().text = "Damage: " + _weaponObject.GetComponent<WeaponBehaviourScript>().DamagePerShoot.ToString();
                if (t.name == "Weapon FireRate Text") t.GetComponent<Text>().text = "Fire Rate: " + WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().cooldown.ToString();
                if (t.name == "Weapon Description Text") t.GetComponent<Text>().text = WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().description;

                if (t.name == "Btn_PreviousWeapon") {
                    if (WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().PreviousWeapon != null) {
                        if (WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().PreviousWeapon.GetComponent<WeaponBehaviourScript>().isBought == true)
                            t.GetComponent<Button>().interactable = true;
                        else t.GetComponent<Button>().interactable = false;
                    }
                    else t.GetComponent<Button>().interactable = false;
                }
                if (t.name == "Btn_NextWeapon") {
                    if (WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon != null) {
                        if (WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon.GetComponent<WeaponBehaviourScript>().isBought == true) 
                            t.GetComponent<Button>().interactable = true;
                        else t.GetComponent<Button>().interactable = false;
                    }
                    else t.GetComponent<Button>().interactable = false;
                }

                if (t.name == "Btn_Back") {
                    bool ActivateUpgrade = false;
                    //float BackButtonStandartPos = -1;
                    if (BackButtonStandartPos == -1) BackButtonStandartPos = t.GetComponent<RectTransform>().anchoredPosition.x;

                    if (WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon != null) {
                        if (WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon.GetComponent<WeaponBehaviourScript>().isBought == false) {
                            ActivateUpgrade = true;
                        }
                    }
                    if (ActivateUpgrade == false) {
                        foreach (Transform upgradeBtn in WeaponInfoSrceen.transform)  if (upgradeBtn.name == "Btn_UpgradeWeapon") upgradeBtn.gameObject.SetActive(false);
                        t.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, t.GetComponent<RectTransform>().anchoredPosition.y);
                    }
                    if (ActivateUpgrade == true) {
                        foreach (Transform upgradeBtn in WeaponInfoSrceen.transform) if (upgradeBtn.name == "Btn_UpgradeWeapon") upgradeBtn.gameObject.SetActive(true);
                        t.GetComponent<RectTransform>().anchoredPosition = new Vector2(BackButtonStandartPos, t.GetComponent<RectTransform>().anchoredPosition.y);
                    }
                }
            }

        }
        else {
            Debug.LogError("OpenWeaponInfoScreen hass been called with the object " + _weaponObject.name + ", which has no WeaponBehaviourScript assinged.");
        }
    }

    public void PreviousWeapon_Btn() {
        OpenWeaponInfoScreen(WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().PreviousWeapon);
    }

    public void NextWeapon_Btn() {
        OpenWeaponInfoScreen(WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon);
    }

    public void Btn_UpgradeWeapon() {
        WeaponBehaviourScript _nextwep = WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon.GetComponent<WeaponBehaviourScript>();
        if (GameControllerScript.currendCredits >= _nextwep.price) {
            GameControllerScript.currendCredits -= _nextwep.price;


            _nextwep.isBought = true;
            WeaponsViewGO.GetComponent<WeaponsViewControllerScript>().UpdateWeaponsView();
            UpdateUI();
            OpenWeaponInfoScreen(WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().NextWeapon);
        }
        else Debug.Log("You need some visual feedback that the player has not enouth money to uprgade the weapoon...");
    }

    public void Btn_WeaponSelectScreenBack() {
        WeaponInfoSrceen.SetActive(false);
        WeaponInfoScreenWeaponGo = null;
    }
}