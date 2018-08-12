using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelControllerScript : MonoBehaviour {

    public GameObject ObjectHolderGo;

    public GameObject[] Waves;
    //private GameObject activeWave;

    //[NonSerialized]
    public bool WaveActive = true;

    void Start() {
        StartCoroutine(InstantiatePlayerAfterTime(0.00001f));
        StartCoroutine(StartAfterTime(1f));
    }

    IEnumerator InstantiatePlayerAfterTime(float duration) {
        yield return new WaitForSeconds(duration);

        if (GameObject.FindGameObjectWithTag("Player") == null) {
            Debug.Log("Spawned Player");
            Instantiate(ObjectHolderGo.GetComponent<ObjectHolder>().PlayerShips[ObjectHolder.GetPlayerShipIndex(PlayerControllerScript.Ships.Standart)]);
        }
    }

    IEnumerator StartAfterTime(float duration) {
        yield return new WaitForSeconds(duration);
        WaveActive = false;
    }

    void Update() {
        if (WaveActive == false) {
            Instantiate(Waves[UnityEngine.Random.Range(0, Waves.Length)]);
            WaveActive = true;
        }
    }
    /*
    IEnumerator DelayWaveStart(float duration) {
        yield return new WaitForSeconds(duration);
        
    }
    */
}
