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

            HealthBarObject.transform.localScale = new Vector3(HealthBarObject.transform.localScale.x * transform.lossyScale.normalized.x, HealthBarObject.transform.localScale.y * transform.lossyScale.normalized.y, HealthBarObject.transform.localScale.z * transform.lossyScale.normalized.z); //for Mirroring using the scale
            //HealthBarObject.transform.localScale = transform.lossyScale;
        }
        if (EnemyObject == null) {
            Debug.LogError("The EnemyParent " + transform.gameObject.name + " never had an EnemyObject assinged. It destroyed itself...");
        }
        else {
            EnemyObject.transform.localScale = new Vector3(EnemyObject.transform.localScale.x * (1 / transform.lossyScale.x), EnemyObject.transform.localScale.y * 1 / (transform.lossyScale.y), EnemyObject.transform.localScale.z * 1 / (transform.lossyScale.z)); //for Mirroring using the scale
            //EnemyObject.transform.localScale = new Vector3(EnemyObject.transform.localScale.x * transform.lossyScale.normalized.x, EnemyObject.transform.localScale.y * transform.lossyScale.normalized.y, EnemyObject.transform.localScale.z * transform.lossyScale.normalized.z); //for Mirroring using the scale
            //EnemyObject.transform.localScale = transform.lossyScale;
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
