using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorGUIBehaviourScript : MonoBehaviour {

    private Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
	}
	
	void Update () {
        Vector3 newPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        transform.position = newPos;

        if (Input.GetButtonDown("Fire2")) { //this is some hard bullshit cus it relies on Fire2 being the button to switch weapons----------------------------------------------------------------------------------------------------------
            animator.SetTrigger("SwitchWeaponTrigger");
        }
    }
}
