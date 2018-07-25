using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    private LineRenderer lineRenderer;
    [SerializeField]
    private float laserLength = 100f;

	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
	}
	
	void Update () {

        RaycastHit2D[] hits = new RaycastHit2D[10];

        //RaycastHit2D[] hits = new RaycastHit2D[Physics2D.Raycast(transform.position, transform.right, new ContactFilter2D(), new RaycastHit2D[1], Mathf.Infinity)];
        //Debug.Log(Physics2D.Raycast(transform.position, transform.right, new ContactFilter2D(), new RaycastHit2D[1], Mathf.Infinity));

        ContactFilter2D CF = new ContactFilter2D();
        CF.NoFilter();
        Physics2D.Raycast(transform.position, transform.up, CF, hits, Mathf.Infinity);

        foreach (RaycastHit2D hit in hits) {
            if (hit != false) {
                Debug.Log(hit.collider.gameObject.name);
            }
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(0, transform.up * laserLength);
    }
}
