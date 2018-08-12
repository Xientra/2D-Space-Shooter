﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelWaveScript : MonoBehaviour {

    public GameObject EndlessLevelControllerGo;
    public float durationUntilDestroy;

    public bool isActive = true;

	void Start () {
        StartCoroutine(destroyAfterDuration());

    }
	
	void Update () {
		
	}

    IEnumerator destroyAfterDuration() {
        yield return new WaitForSeconds(durationUntilDestroy);
        isActive = false;
        Debug.Log("Wave Ended");
        EndlessLevelControllerGo.GetComponent<EndlessLevelControllerScript>().WaveActive = false;
        Destroy(this.gameObject);
    }
}
