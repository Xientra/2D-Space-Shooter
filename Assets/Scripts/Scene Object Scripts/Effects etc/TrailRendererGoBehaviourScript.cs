using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererGoBehaviourScript : MonoBehaviour {

    [SerializeField]
    private float duration = 1f;

	void Start () {
		
	}
	
	void Update () {

        if (transform.parent == null) {
            StartCoroutine(destroyAfterTime(duration));
        }
	}

    IEnumerator destroyAfterTime(float _duration) {
        yield return new WaitForSeconds(_duration);
        Destroy(this.gameObject);
    }
}
