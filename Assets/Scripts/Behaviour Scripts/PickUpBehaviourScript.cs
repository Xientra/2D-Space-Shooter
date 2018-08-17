using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviourScript : MonoBehaviour {

    Vector3 startDirection = new Vector3(1, 0, 0);

    public int CreditValue = 1;

    public int IndexOfCreditValue = 1;
    public int[] CreditValues = { 1, 10, 100, 1000};

    public enum PickUpTypes { Credit, HealthUp, Regeneration, FireRateUp, DamageUp, Invincibility, SloMo }
    public PickUpTypes thisPickUpType;


    [SerializeField]
    private float TimeBeforeDestroy = 10f;
    [SerializeField]
    private float MinDistaceToPlayer = 5f;
    [SerializeField]
    private float MaxSpeedToPlayer = 1f;
    [SerializeField]
    private float turnspeed = 10f;

    [SerializeField]
    private float accToPlayer = 1f;
    [SerializeField]
    private float accaccToPlayer = 0.05f;
    [SerializeField]
    private float deaccToPlayer = 0.01f;

    private Vector3 movehere;

    void Start() {
        StartCoroutine(destroyAfterTime());

        startDirection = transform.rotation * startDirection;
        transform.rotation = Quaternion.identity;
    }

    void Update() {

        /*
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < MinDistaceToPlayer) {
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, movementSpeedToPlayer * accToPlayer * Time.deltaTime); //Move Towarts Player
        }
        transform.position += speed_ * 1 * accToPlayer * Time.deltaTime;
        */

        movehere += startDirection;

        //To Player Movement
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < MinDistaceToPlayer) {
            startDirection = new Vector2(0, 0);

            if (!(accToPlayer >= 1)) accToPlayer += accaccToPlayer;

            //transform.right = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position; // Look At Player


            //movehere = GameObject.FindGameObjectWithTag("Player").transform.position;
            movehere = Vector2.Lerp(movehere, GameObject.FindGameObjectWithTag("Player").transform.position, turnspeed * Time.deltaTime);
        }
        else {
            if (!(accToPlayer <= 0)) accToPlayer -= deaccToPlayer;
        }
        transform.position = Vector2.MoveTowards(transform.position, movehere, MaxSpeedToPlayer * accToPlayer * Time.deltaTime); //Move Towarts Player
    }

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(TimeBeforeDestroy);
        Destroy(this.gameObject);
    }

    //int i = fuck;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (thisPickUpType == PickUpTypes.Credit) {
                GameControllerScript.currendCredits += CreditValues[IndexOfCreditValue];
                Destroy(this.gameObject);
            }
        }
    }
}
