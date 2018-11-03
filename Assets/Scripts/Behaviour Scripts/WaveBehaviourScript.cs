﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviourScript : MonoBehaviour {

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

        //this.transform.parent.transform.gameObject.GetComponent<EndlessLevelControllerScript>().WaveActive = false;
        GameObject.FindGameObjectWithTag("Endless Level Controller").GetComponent<WaveControllerScript>().WaveActive = false;
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}