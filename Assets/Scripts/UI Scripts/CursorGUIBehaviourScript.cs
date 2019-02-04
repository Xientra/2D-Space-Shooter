using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorGUIBehaviourScript : MonoBehaviour {
	void Update () {
        Vector3 newPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        transform.position = newPos;
    }
}