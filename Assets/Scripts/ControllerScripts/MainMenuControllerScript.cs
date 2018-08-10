using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControllerScript : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
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
}
