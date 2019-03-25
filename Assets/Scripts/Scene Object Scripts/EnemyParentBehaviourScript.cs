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


        //for Mirroring and scaling of the Enemies animation
        if (HealthBarObject == null) {
            Debug.LogError("The EnemyParent " + transform.gameObject.name + " couldn't find any object to assing as the HealthBarObject");
        }
        else {
            HealthBarObject.transform.localScale = new Vector3(HealthBarObject.transform.localScale.x / transform.lossyScale.x, HealthBarObject.transform.localScale.y / transform.lossyScale.y, HealthBarObject.transform.localScale.z / transform.lossyScale.z); 
        }
        if (EnemyObject == null) {
            Debug.LogError("The EnemyParent " + transform.gameObject.name + " never had an EnemyObject assinged. It destroyed itself...");
        }
        else {
            EnemyObject.transform.localScale = new Vector3(EnemyObject.transform.localScale.x / transform.lossyScale.x, EnemyObject.transform.localScale.y / transform.lossyScale.y, EnemyObject.transform.localScale.z / transform.lossyScale.z);
        }
    }
	
	void Update () {

        if (EnemyObject == null)
            Destroy(this.gameObject);
        else {
            HealthBarObject.transform.position = EnemyObject.transform.position;
            HealthBarObject.transform.up = Vector3.up;
        }
    }
}