using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererGoBehaviourScript : MonoBehaviour {

    //[SerializeField]
    private float duration = 1f;

	void Start () {
		
	}
	
	void Update () {
        if (transform.parent == null) {
            StartCoroutine(destroyAfterTime());
        }
	}

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
