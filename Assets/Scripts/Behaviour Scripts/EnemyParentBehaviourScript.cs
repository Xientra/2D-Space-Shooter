using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentBehaviourScript : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        if (GetComponentInChildren<EnemyBehaviourScript>() == null)
            Destroy(this.gameObject);
	}
}
