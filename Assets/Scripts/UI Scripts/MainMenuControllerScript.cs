using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControllerScript : MonoBehaviour {

    public GameObject StartMenu;
    //public GameObject StoryMenu;
    public GameObject LevelSelect;
    public GameObject OutfitterMenu;
    public GameObject WeaponInfoScreen;
    //public GameObject OptionsMenu;

    //Outfitter Elements
    private GameObject FirstWeaponDropdownGO;
    private GameObject SecondWeaponDropdownGO;
    private GameObject WeaponsViewGO;
    private GameObject WeaponInfoScreenWeaponGo = null;
    //for the movement of the back and the upgrade buttons
    private float BackButtonStandartPos = -1;


    //public static GameObject firstWeaponGO;
    //public static GameObject secondWeaponGO;

    public static float NewWeaponPrice = 200f;


    void Start() {

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

        StartCoroutine(UpdateUIAfterOneFrame());
    }
    IEnumerator UpdateUIAfterOneFrame() {
        yield return 0;
        UpdateUI();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (WeaponInfoScreen.activeSelf == true) {
                Btn_WeaponSelectScreenBack();
            }
        }
    }

    private void UpdateUI() {
        if (StartMenu.activeSelf == true) {
            foreach (Transform t in StartMenu.transform) {
                if (t.name == "Hight Score Text") t.GetComponent<Text>().text = "Hight Score: " + System.Environment.NewLine + GameControllerScript.HightScore.ToString();
            }
        }

        if (LevelSelect.activeSelf == true) {
            UpdateLevelButtons();
        }

        if (OutfitterMenu.activeSelf == true) {

            WeaponsViewGO.GetComponent<WeaponsViewControllerScript>().UpdateWeaponsView();

            foreach (Transform t in OutfitterMenu.transform) {
                //if (t.CompareTag("Currend Credits UI")) t.GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
                //GameObject.FindGameObjectsWithTag("First Weapon UI")[0].GetComponent<Text>().text = "First Weapon:" + System.Environment.NewLine + firstWeapon.ToString();
                //GameObject.FindGameObjectsWithTag("Second Weapon UI")[0].GetComponent<Text>().text = "Second Weapon:" + System.Environment.NewLine + secondWeapon.ToString();

                if (t.name == "Currend Credits Text") t.GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
                if (t.name == "Btn_BuyNewWeapon") t.GetComponentsInChildren<Transform>()[1].GetComponent<Text>().text = NewWeaponPrice.ToString();
            }

            if (FirstWeaponDropdownGO != null) {
                int counter1 = 0;
                FirstWeaponDropdownGO.GetComponent<Dropdown>().options.Clear();
                foreach (GameObject pWep in ObjectHolder._PlayerWeapons)
                    if (pWep != null) {
                        if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true) {
                            FirstWeaponDropdownGO.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pWep.GetComponent<WeaponBehaviourScript>().weaponName));
                            //if (GameControllerScript.PlayerFirstWeapon != null)
                                if (pWep.GetComponent<WeaponBehaviourScript>().WeaponType == GameControllerScript.PlayerFirstWeapon.GetComponent<WeaponBehaviourScript>().WeaponType) {
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
                            //if (GameControllerScript.PlayerSecondWeapon != null)
                            if (pWep.GetComponent<WeaponBehaviourScript>().WeaponType == GameControllerScript.PlayerSecondWeapon.GetComponent<WeaponBehaviourScript>().WeaponType) {
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
    public void Btn_Story() {
        Debug.Log("probably never...");
    }

    public void Btn_LevelSelect() {
        StartMenu.SetActive(false);
        //StoryMenu.SetActive(false);
        LevelSelect.SetActive(true);
        OutfitterMenu.SetActive(false);
        WeaponInfoScreen.SetActive(false);
        //OptionsMenu.SetActive(false);

        UpdateUI();
    }

    public void Btn_Endless() {
        SceneManager.LoadScene("Endless Level");
    }

    public void Btn_Outfitter() {
        StartMenu.SetActive(false);
        //StoryMenu.SetActive(false);
        LevelSelect.SetActive(false);
        OutfitterMenu.SetActive(true);
        WeaponInfoScreen.SetActive(false);
        //OptionsMenu.SetActive(false);

        UpdateUI();
    }

    public void Btn_Options() {
        Debug.Log("lol no");
    }

    public void Btn_Quit() {
        GameControllerScript.instance.SaveGame();
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Btn_BackToMainMenu() {
        StartMenu.SetActive(true);
        //StoryMenu.SetActive(false);
        LevelSelect.SetActive(false);
        OutfitterMenu.SetActive(false);
        //OptionsMenu.SetActive(false);

        UpdateUI();
    }

    //¯\_(ツ)_/¯
    public void Btn_UnlockAllWeapons() {
        GameControllerScript.currendCredits += 1000f;
        /*
        foreach (GameObject playerWeapon in ObjectHolder._PlayerWeapons) {
            playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = true;
        }
        */
        UpdateUI();
    }


    /*-----------------------------------------Level Select-----------------------------------------------*/

    public void UpdateLevelButtons() {
        foreach (Transform t in LevelSelect.transform) {
            //if (t.name == "Level1_Btn") t.gameObject.GetComponent<Button>().interactable = true; //GameControllerScript.LevelProgress[0 - 1];
            if (t.name == "Level2_Btn") t.gameObject.GetComponent<Button>().interactable = GameControllerScript.LevelProgress[1 - 1];
            if (t.name == "Level3_Btn") t.gameObject.GetComponent<Button>().interactable = GameControllerScript.LevelProgress[2 - 1];
        }
    }

    public void Btn_Level1() {
        SceneManager.LoadScene("Level 1");
    }
    public void Btn_Level2() {
        SceneManager.LoadScene("Level 2");
    }
    public void Btn_Level3() {
        SceneManager.LoadScene("Level 3");
    }


    /*-----------------------------------------Outfitter Menu-----------------------------------------------*/
    public void Btn_BuyNewWeapon() {
        List<GameObject> buyableWeapons = new List<GameObject>();
        foreach (GameObject WepGo in ObjectHolder._PlayerWeapons) {
            if (WepGo != null) {
                if (WepGo.GetComponent<WeaponBehaviourScript>().WeaponLevel == WeaponBehaviourScript.WeaponLevels._1 && WepGo.GetComponent<WeaponBehaviourScript>().isBought == false)
                    buyableWeapons.Add(WepGo);
            }
        }

        if (buyableWeapons.Count != 0) {
            if (GameControllerScript.currendCredits >= NewWeaponPrice) {

                GameObject randomWeponGo = buyableWeapons[Random.Range(0, buyableWeapons.Count)];
                randomWeponGo.GetComponent<WeaponBehaviourScript>().isBought = true;

                GameControllerScript.currendCredits -= NewWeaponPrice;
                NewWeaponPrice += 100;
               
                OpenWeaponInfoScreen(randomWeponGo);
                UpdateUI();
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
                        GameControllerScript.PlayerFirstWeapon = pWep;
                    counter++;
                }
        }

        Debug.Log("first weapon = "+GameControllerScript.PlayerFirstWeapon.name);
    }

    public void OnValueChangeSecondWeaponDropdown() {
        int i = SecondWeaponDropdownGO.GetComponent<Dropdown>().value;
        int counter = 0;

        foreach (GameObject pWep in ObjectHolder._PlayerWeapons) {
            if (pWep != null)
                if (pWep.GetComponent<WeaponBehaviourScript>().isBought == true) {
                    if (counter == i)
                        GameControllerScript.PlayerSecondWeapon = pWep;
                    counter++;
                }
        }

        Debug.Log("second weapon = "+GameControllerScript.PlayerSecondWeapon.name);
    }

    

    /*-----------------------------------------Weapon Info Screen-----------------------------------------------*/
    //Is called from WeaponsViewElementDataScript
    public void OpenWeaponInfoScreen(GameObject _weaponObject) {
        if (_weaponObject.GetComponent<WeaponBehaviourScript>() != null) {
            StartMenu.SetActive(false);
            //StoryMenu.SetActive(false);
            LevelSelect.SetActive(false);
            OutfitterMenu.SetActive(true);
            WeaponInfoScreen.SetActive(true);
            //OptionsMenu.SetActive(false);

            UpdateUI();

            WeaponInfoScreenWeaponGo = _weaponObject;

            foreach (Transform t in WeaponInfoScreen.transform) {
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
                        foreach (Transform upgradeBtn in WeaponInfoScreen.transform)  if (upgradeBtn.name == "Btn_UpgradeWeapon") upgradeBtn.gameObject.SetActive(false);
                        t.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, t.GetComponent<RectTransform>().anchoredPosition.y);
                    }
                    if (ActivateUpgrade == true) {
                        foreach (Transform upgradeBtn in WeaponInfoScreen.transform) if (upgradeBtn.name == "Btn_UpgradeWeapon") upgradeBtn.gameObject.SetActive(true);
                        t.GetComponent<RectTransform>().anchoredPosition = new Vector2(BackButtonStandartPos, t.GetComponent<RectTransform>().anchoredPosition.y);
                    }
                }
            }

        }
        else {
            Debug.LogError("OpenWeaponInfoScreen has been called with the object " + _weaponObject.name + ", which has no WeaponBehaviourScript assinged.");
        }
    }

    public void Btn_EquipAsFirstWeapon() {
        GameControllerScript.PlayerFirstWeapon = WeaponInfoScreenWeaponGo;
        UpdateUI();
    }

    public void Btn_EquipAsSecondWeapon() {
        GameControllerScript.PlayerSecondWeapon = WeaponInfoScreenWeaponGo;
        UpdateUI();
    }

    public void Btn_PreviousWeapon() {
        OpenWeaponInfoScreen(WeaponInfoScreenWeaponGo.GetComponent<WeaponBehaviourScript>().PreviousWeapon);
    }

    public void Btn_NextWeapon() {
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
        WeaponInfoScreen.SetActive(false);
        WeaponInfoScreenWeaponGo = null;
        UpdateUI();
    }
}