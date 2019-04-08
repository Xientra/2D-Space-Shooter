using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveControllerScript : MonoBehaviour {

    [SerializeField]
    private bool isActive = true;

    public bool isEndless = false;

    public int LevelNumber = -1;
    public int rewardonComplete = 0;

    [Space]

    //public GameObject ObjectHolderGo;
    public GameObject activeWave;

    [NonSerialized]
    public bool WaveActive = true;
    private int lastWaveIndex = 0;
    private int childPosition = 1;

    //Only Debug
    private int PastSecondsSinceStart = 0;

    void Start() {
        if (LevelNumber == -1 && isEndless == false) {
            Debug.LogError("Level Number of " + gameObject.name + " has no valid Number assinged.");
        }

        foreach (Transform child in transform) {

            child.gameObject.SetActive(false);
        }

        //StartCoroutine(StartAfterTime(1f));

        StartCoroutine(DisplayPastSeconds());
    }

    IEnumerator StartAfterTime(float duration) {
        yield return new WaitForSeconds(duration);
        WaveActive = false;
    }

    void Update() {
        if (activeWave == null) {
            WaveActive = false;
        }

        if (isActive == true) {
            if (WaveActive == false) {
                int counter = 1;

                if (isEndless == true)
                    while (childPosition == lastWaveIndex)
                        childPosition = UnityEngine.Random.Range(1, transform.childCount + 1);

                foreach (Transform child in transform) {
                    if (counter == childPosition) {
                        activeWave = child.gameObject;

                        if (isEndless == true)
                            activeWave = Instantiate(child.gameObject);
                        activeWave.SetActive(true);
                    }
                    counter++;
                }
                WaveActive = true;

                lastWaveIndex = childPosition;
            }

        }

        if (transform.childCount == 0) {

            StartCoroutine(WinLevel());
        }
    }

    private IEnumerator WinLevel() {

        GameControllerScript.LevelProgress[LevelNumber - 1] = true;

        GameControllerScript.currendCredits += rewardonComplete;

        Debug.Log("YOU WON!!");
        yield return new WaitForSeconds(5f);

        

        SceneManager.LoadScene("Main Menu");
    }


    /*------------------------------Displays Time-----------------------------------------*/
    IEnumerator DisplayPastSeconds() {
        yield return new WaitForSeconds(1);
        this.gameObject.name = "Wave Controller  -( " + PastSecondsSinceStart.ToString() + " )-";
        PastSecondsSinceStart++;
        StartCoroutine(DisplayPastSeconds());
    }
}