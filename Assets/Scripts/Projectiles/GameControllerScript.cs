using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    public static float currendCredits = 0f;
    public static bool UsingGamepad = true;

    void Start () {
		
	}
	
	void Update () {
        float _MaxHealth = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().MaxHealth;
        float _currendHealth = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().currendHealth;
        PlayerControllerScript.Weapons _currentWeapon = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().currentWeapon;

        //Update UI
        if (onlyOnGOwithTag("Health Bar UI")) {
            GameObject.FindGameObjectsWithTag("Health Bar UI")[0].GetComponent<Slider>().maxValue = _MaxHealth;
            GameObject.FindGameObjectsWithTag("Health Bar UI")[0].GetComponent<Slider>().value = _currendHealth;
        }
        if (onlyOnGOwithTag("Currend Weapon UI")) {
            GameObject.FindGameObjectsWithTag("Currend Weapon UI")[0].GetComponent<Text>().text = "Weapon:" + System.Environment.NewLine + _currentWeapon.ToString();
        }
        if (onlyOnGOwithTag("Currend Credits UI")) {
            GameObject.FindGameObjectsWithTag("Currend Credits UI")[0].GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + currendCredits.ToString();
        }
    }

    public static bool onlyOnGOwithTag(string _tag) {
        bool r = false;
        if (GameObject.FindGameObjectsWithTag(_tag).Length != 0) {
            if (GameObject.FindGameObjectsWithTag(_tag).Length > 1) {
                Debug.LogError("There are more than one " + _tag + " objects in the scene.");
            }
            else {
                r = true;
            }
        }
        else {
            Debug.LogError("There is no " + _tag + " in the scene.");
        }
        return r;
    }
}
