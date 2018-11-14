using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviourScript : MonoBehaviour {

    public float durationUntilDestroy;

    //public bool isActive = true;

    private int PastSecondsSinceStart = 0;
    private string OriginalObjectName;

    void Start () {
        OriginalObjectName = this.gameObject.name;
        StartCoroutine(DisplaySecondsTillDestruction());

        StartCoroutine(destroyAfterDuration());
    }
	
	void Update () {
	}

    IEnumerator destroyAfterDuration() {
        yield return new WaitForSeconds(durationUntilDestroy);
        //isActive = false;

        //this.transform.parent.transform.gameObject.GetComponent<EndlessLevelControllerScript>().WaveActive = false;
        //GameObject.FindGameObjectWithTag("Endless Level Controller").GetComponent<WaveControllerScript>().WaveActive = false;
        
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator DisplaySecondsTillDestruction() {
        yield return new WaitForSeconds(1);
        this.gameObject.name = OriginalObjectName + "  -( " + PastSecondsSinceStart.ToString() + "/" + durationUntilDestroy.ToString() + " )-";
        PastSecondsSinceStart++;
        StartCoroutine(DisplaySecondsTillDestruction());
    }
}
