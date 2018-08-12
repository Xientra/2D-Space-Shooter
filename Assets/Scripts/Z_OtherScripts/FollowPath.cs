using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    public GameObject ToMove;

    //public GameObject[] waypoint;
    private Node[] nodepoints;

    int currendnode;

    float Timer;
    public float movementSpeed = 1f;

    static Vector3 currendpoition;

	void Start () {
        nodepoints = GetComponentsInChildren<Node>();

        checkNode();


    }

	void Update () {
        DrawLines();
        Timer += Time.deltaTime * movementSpeed;

        if (ToMove.transform.position != currendpoition) {
            ToMove.transform.position = Vector3.Lerp(ToMove.transform.position, currendpoition, Timer);
            //ToMove.transform.position = Vector3.MoveTowards(ToMove.transform.position, currendpoition, 0.05f);
        }
        else {
            if (currendnode < nodepoints.Length - 1) {
                currendnode += 1;
                checkNode();
            }
        }
	}

    void checkNode() {
        if (currendnode < nodepoints.Length - 1) 
            Timer = 0f;
            currendpoition = nodepoints[currendnode].transform.position;
        
    }

    void DrawLines() {
        for (int i = 0; i < nodepoints.Length; i++) {
            if (i < nodepoints.Length - 1) {
                Debug.DrawLine(nodepoints[i].transform.position, nodepoints[i + 1].transform.position, Color.green);
            }
        }
    }
}
