using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControllerScript : MonoBehaviour {

    [SerializeField]
    private GameObject OnWinEffect;
    private bool hasWon = false;

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
        
        if (isEndless == true)
            while (childPosition == lastWaveIndex)
                childPosition = UnityEngine.Random.Range(1, transform.childCount + 1);

        //StartCoroutine(StartAfterTime(1f));

        StartCoroutine(DisplayPastSeconds());
    }

    IEnumerator StartAfterTime(float _time) {
        yield return new WaitForSeconds(_time);
        WaveActive = false;
    }

    void Update() {
        if (hasWon == false) {
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
    }

    private IEnumerator WinLevel() {
        float _delay = 7f;

        GameControllerScript.LevelProgress[LevelNumber - 1] = true;
        GameControllerScript.currendCredits += rewardonComplete;
        hasWon = true;

        Instantiate(OnWinEffect, OnWinEffect.transform.position, OnWinEffect.transform.rotation);

        //Wait and SloMo
        yield return new WaitForSecondsRealtime(_delay * 0.6f);
        float n = 50f;
        for (int i = 1; i <= n; i++) {
            yield return new WaitForSecondsRealtime((_delay * 0.4f) / n);
            if (Time.timeScale - (1 / n) < 0)
                Time.timeScale = 0;
            else
                Time.timeScale -= (1 / n);
        }

        GameControllerScript.instance.SaveGame();
        GameObject.FindGameObjectWithTag("InGameUI").GetComponent<InGameUIControllerScript>().OpenInGameWinMenu();
    }

    /*------------------------------Displays Time-----------------------------------------*/
    IEnumerator DisplayPastSeconds() {
        yield return new WaitForSeconds(1);
        this.gameObject.name = "Wave Controller  -( " + PastSecondsSinceStart.ToString() + " )-";
        PastSecondsSinceStart++;
        StartCoroutine(DisplayPastSeconds());
    }
}