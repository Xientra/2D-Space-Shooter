using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public float durationUntilDestroy;

    //public bool isActive = true;

    private int PastSecondsSinceStart = 0;
    private string OriginalObjectName;

    void Start () {
        OriginalObjectName = this.gameObject.name;
        StartCoroutine(DisplaySecondsTillDestruction());

        StartCoroutine(destroyAfterDuration(durationUntilDestroy));
    }
	
	void Update () {
        if (transform.childCount == 0) {
            StartCoroutine(destroyAfterDuration(1f));
        }

	}

    IEnumerator destroyAfterDuration(float _duration) {
        yield return new WaitForSeconds(_duration);
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
