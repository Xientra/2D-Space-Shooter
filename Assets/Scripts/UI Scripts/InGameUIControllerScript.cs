using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIControllerScript : MonoBehaviour {

    public static InGameUIControllerScript activeInstance;

    public GameObject InGameUI;
    public GameObject InGameExitMenu;
    public GameObject InGameDeathMenu;
    public GameObject InGameWinMenu;

    [Space(5)]
    public GameObject TutorialText;


    private bool UpdateUI = false; //to not update the UI in the first frame

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
            Debug.LogWarning("There was more then one InGameUI in the scene");
        }
    }

    void Start() {
        if (InGameUI == null)
            InGameUI = GameObject.FindGameObjectWithTag("InGameUI");

        foreach (Transform t in InGameUI.transform) {
            if (InGameExitMenu == null)
                if (t.gameObject.name == "InGameExitMenu") {
                    InGameExitMenu = t.gameObject;
                    InGameExitMenu.SetActive(false);
                }
            if (InGameDeathMenu == null)
                if (t.gameObject.name == "InGameDeathMenu") {
                    InGameDeathMenu = t.gameObject;
                    InGameDeathMenu.SetActive(false);
                }
        }

        StartCoroutine(DoStuffAfterOneFrame());
    }

    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;
        UpdateUI = true;
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (InGameExitMenu.activeSelf == false) {
                OpenInGameExitMenu();
                PlayButtonSound();
            }
            else {
                Btn_No(); //which deactivates the InGameExitMenu and starts Time again
            }
        }

        if (UpdateUI == true)
            UpdateInGameUI();
    }

    public void UpdateInGameUI() {

        foreach (Transform t in InGameUI.transform) {
            if (GameControllerScript.instance.PlayerInstance != null) {
                PlayerBehaviourScript pbs = GameControllerScript.instance.PlayerInstance.GetComponent<PlayerBehaviourScript>();

                if (t.CompareTag("Health Bar UI")) {
                    t.GetComponent<Slider>().maxValue = pbs.MaxHealth;
                    t.GetComponent<Slider>().value = pbs.ChangeHealthBy(0);
                }
                if (t.name == "Cooldown Bar 1")
                    t.gameObject.GetComponent<Slider>().value = pbs.GetPercentUnitlCooldown1();
                if (t.name == "Cooldown Bar 2")
                    t.gameObject.GetComponent<Slider>().value = pbs.GetPercentUnitlCooldown2();
            }
            if (t.gameObject.name == "First Weapon Text")
                t.gameObject.GetComponent<Text>().text = "First Weapon:" + System.Environment.NewLine + GameControllerScript.PlayerFirstWeapon.GetComponent<WeaponBehaviourScript>().weaponName;
            if (t.gameObject.name == "Second Weapon Text")
                t.gameObject.GetComponent<Text>().text = "Second Weapon:" + System.Environment.NewLine + GameControllerScript.PlayerSecondWeapon.GetComponent<WeaponBehaviourScript>().weaponName;
            if (t.gameObject.name == "Currend Credits Text")
                t.gameObject.GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + GameControllerScript.currendCredits.ToString();
            if (t.gameObject.name == "Currend Score Text")
                t.gameObject.GetComponent<Text>().text = "Score: " + System.Environment.NewLine + GameControllerScript.currendScore.ToString();
        }
    }

    private void PlayButtonSound() {
        AudioControllerScript.activeInstance.PlaySound("ButtonPress");
    }

    public void OpenInGameExitMenu() {
        OpenInGameMenu(InGameExitMenu);
    }

    public void OpenInGameDeathMenu() {
        foreach (Transform t in InGameDeathMenu.transform) {
            if (t.name == "HightScore_Text") t.GetComponent<Text>().text = "Hightscore: " + System.Environment.NewLine + GameControllerScript.HightScore.ToString();
            if (t.name == "YourScore_Text") t.GetComponent<Text>().text = "Your Score: " + System.Environment.NewLine + GameControllerScript.currendScore.ToString();
        }

        OpenInGameMenu(InGameDeathMenu);
    }

    public void OpenInGameWinMenu() {
        OpenInGameMenu(InGameWinMenu);
    }
    
    /*-------------------------------------------InGame Menu Buttons-------------------------------------------------------*/

    public void Btn_Yes() {
        SceneManager.LoadScene("Main Menu");
        GameControllerScript.PauseGame(false);

        PlayButtonSound();
    }
    public void Btn_No() {
        CloseInGameMenu(InGameExitMenu);

        PlayButtonSound();
    }

    public void Btn_Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameControllerScript.PauseGame(false);

        PlayButtonSound();
    }
    public void Btn_Exit() {
        SceneManager.LoadScene("Main Menu");
        GameControllerScript.PauseGame(false);

        PlayButtonSound();
    }

    private void OpenInGameMenu(GameObject _menuGo) {
        _menuGo.SetActive(true);
        GameControllerScript.PauseGame(true);
        Cursor.visible = true;
    }
    private void CloseInGameMenu(GameObject _menuGo) {
        _menuGo.SetActive(false);
        GameControllerScript.PauseGame(false);
        Cursor.visible = false;
    }

    public void OpenTutorialText(string _text, float _time = 5f) {
        StartCoroutine(StartOpenTutorialText(_text, _time));
    }

    private IEnumerator StartOpenTutorialText(string _text, float _time = 5f) {

        TutorialText.GetComponent<Text>().text = _text;

        TutorialText.SetActive(true);

        yield return new WaitForSeconds(_time);

        TutorialText.SetActive(false);
    }
}