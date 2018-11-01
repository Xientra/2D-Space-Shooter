using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControllerScript : MonoBehaviour {

    public GameObject ObjectHolderGo;
    public GameObject activeWave;

    public bool isEndless = false;

    [NonSerialized]
    public bool WaveActive = true;

    void Start() {
        foreach (Transform child in transform) {

            child.gameObject.SetActive(false);
        }

        StartCoroutine(InstantiatePlayerAfterOneFrame());
        StartCoroutine(StartAfterTime(1f));
    }

    IEnumerator InstantiatePlayerAfterOneFrame() {
        yield return null;

        if (GameObject.FindGameObjectWithTag("Player") == null) {
            Debug.Log("Spawned Player");
            Instantiate(ObjectHolderGo.GetComponent<ObjectHolder>().PlayerShips[ObjectHolder.GetPlayerShipIndex(PlayerBehaviourScript.Ships.Standart)]);
        }
    }

    IEnumerator StartAfterTime(float duration) {
        yield return new WaitForSeconds(duration);
        WaveActive = false;
    }

    void Update() {
        if (WaveActive == false) {
            int counter = 1;
            int childPosition = 1;
            if (isEndless == true)
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
        }
    }
}