using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviourScript : MonoBehaviour {

    public float CreditValue = 1;

    /*--Use this if you want to get specific values from other Scripts--*/
    //public int IndexOfCreditValue = 1;
    //public int[] CreditValues = { 1, 10, 100, 1000};

    public enum PickUpTypes { Credit, HealthUp, Regeneration, FireRateUp, DamageUp, Invincibility, SloMo }
    public PickUpTypes thisPickUpType;

    [SerializeField]
    private Vector3 speed = new Vector3();
    [SerializeField]
    private float startspeed = 5f;
    [SerializeField]
    private float speedToPlayer = 0.6f;
    [SerializeField]
    private float maxSpeedToPlayer = 7.5f;

    [SerializeField]
    private float TimeBeforeDestroy = 10f;
    [SerializeField]
    private float MinDistaceToPlayer = 5f;

    private bool MoveToPlayer = false;

    void Start() {
        StartCoroutine(destroyAfterTime());
        transform.rotation = Quaternion.identity;

        /*--Create Random Direction For Start--*/
        float _startspeed = startspeed / 2;
        speed = new Vector3(Random.Range(-_startspeed, _startspeed), Random.Range(-_startspeed, _startspeed), 0);
    }

    void Update() {
    }

    private void FixedUpdate() {
        AccelerationMovement();
    }

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(TimeBeforeDestroy);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        /*
        if (collision.CompareTag("Player")) {
            if (thisPickUpType == PickUpTypes.Credit) {
                GameControllerScript.currendCredits += CreditValue;//CreditValues[IndexOfCreditValue];
                //Destroy(this.gameObject);
            }
        }
        */
        if (collision.gameObject.CompareTag("Enemy Limiter")) {
            Destroy(this.gameObject);
        }
    }

    void AccelerationMovement() {
        if (MoveToPlayer == false) {
            if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < MinDistaceToPlayer) {
                MoveToPlayer = true;
            }
        }
        else {
            Vector3 PlayerDirection = Vector3.Normalize(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);

            //speed.Normalize();
            speed += PlayerDirection * speedToPlayer;
        }
        speed = new Vector3(Mathf.Clamp(speed.x, -maxSpeedToPlayer, maxSpeedToPlayer), Mathf.Clamp(speed.y, -maxSpeedToPlayer, maxSpeedToPlayer), 0);

        transform.position += speed * Time.deltaTime;
    }
}