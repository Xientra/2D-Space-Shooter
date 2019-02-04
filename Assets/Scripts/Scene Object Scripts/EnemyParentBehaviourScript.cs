using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentBehaviourScript : MonoBehaviour {

    public GameObject EnemyObject;
    public GameObject HealthBarObject;   

	void Start () {
        if (HealthBarObject == null) {
            foreach (Transform t in GetComponentsInChildren<Transform>()) {
                if (t.gameObject.CompareTag("HealthBar"))
                    HealthBarObject = t.gameObject;
            }
        }
        if (EnemyObject == null) {
            Debug.LogError("The EnemyParent "+transform.gameObject.name+" has no EnemyObject assinged. It destroyed itself...");
        }
    }
	
	void Update () {

        if (EnemyObject == null)
            Destroy(this.gameObject);
        else {
            HealthBarObject.transform.position = EnemyObject.transform.position;
        }
    }
}
