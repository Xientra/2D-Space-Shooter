using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIControllerScript : MonoBehaviour {

    public GameObject InGameUI;
    public GameObject InGameExitMenu;
    public GameObject InGameDeathMenu;

    private bool UpdateUI = false; //to not update the UI in the first frame

    void Start () {
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

    void Update () {
        if (Input.GetButtonDown("Cancel")) {
            if (InGameExitMenu.activeSelf == false) {
                OpenInGameExitMenu();
            }
            else {
                Btn_No(); //which deactivates the InGameExitMenu and starts Time again
            }
        }

        if (UpdateUI == true)
            UpdateInGameUI();
    }

    public void UpdateInGameUI() {

        if (GameObject.FindGameObjectWithTag("Player") != null) {
            float _MaxHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviourScript>().MaxHealth;
            GameObject.FindGameObjectWithTag("Health Bar UI").GetComponent<Slider>().maxValue = _MaxHealth;

            float _currendHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviourScript>().currendHealth;
            GameObject.FindGameObjectWithTag("Health Bar UI").GetComponent<Slider>().value = _currendHealth;

            GameObject.FindGameObjectWithTag("Cooldown UI").GetComponent<Slider>().value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviourScript>().GetPercentUnitCooldown();
        }

        foreach (Transform t in InGameUI.transform) {
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

    public void OpenInGameExitMenu() {
        InGameExitMenu.SetActive(true);
        GameControllerScript.PauseGame(true);
    }

    public void OpenInGameDeathMenu() {
        InGameDeathMenu.SetActive(true);
        GameControllerScript.PauseGame(true);
    }

    /*-------------------------------------------InGame Menu-------------------------------------------------------*/

    public void Btn_Yes() {
        SceneManager.LoadScene("Main Menu");
        GameControllerScript.PauseGame(false);
    }
    public void Btn_No() {
        InGameExitMenu.SetActive(false);
        GameControllerScript.PauseGame(false);
    }

    public void Btn_Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameControllerScript.PauseGame(false);
    }
    public void Btn_Exit() {
        SceneManager.LoadScene("Main Menu");
        GameControllerScript.PauseGame(false);
    }
}