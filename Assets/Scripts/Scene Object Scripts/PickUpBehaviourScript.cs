using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviourScript : MonoBehaviour {


    /*--Use this if you want to get specific values from other Scripts--*/
    //public int IndexOfCreditValue = 1;
    //public int[] CreditValues = { 1, 10, 100, 1000};

    public enum PickUpTypes { Credit, HealthUp, Regeneration, FireRateUp, DamageUp, Invincibility, SloMo }
    public PickUpTypes thisPickUpType;

    public GameObject VisualEffect;

    [Header("Stats:")]

    public float CreditValue = 1;
    public float duration = 5f;
    public string tutorialText;

    [Header("Movement Stats:")]

    [SerializeField]
    private Vector3 speed = new Vector3();
    [SerializeField]
    private float startspeed = 5f;
    [SerializeField]
    private float speedToPlayer = 0.6f;
    [SerializeField]
    private float maxSpeedToPlayer = 7.5f;

    private float speedFactor = 1.01f;

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
        //AccelerationMovement();
    }

    private void FixedUpdate() {
        if (Time.timeScale > 0) {
            AccelerationMovement();
        }
    }

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(TimeBeforeDestroy);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Player")) {

            PlayerBehaviourScript _player = collision.gameObject.GetComponent<PlayerBehaviourScript>();
            _player.TakePickUp(this.gameObject);

            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy Limiter")) {
            Destroy(this.gameObject);
        }
    }

    void AccelerationMovement() {
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            if (MoveToPlayer == false) {
                if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < MinDistaceToPlayer) {
                    MoveToPlayer = true;
                }
            }
            else if (MoveToPlayer == true) {
                Vector3 PlayerDirection = Vector3.Normalize(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);

                //speed.Normalize();
                speed += PlayerDirection * speedToPlayer;

                speedToPlayer *= (speedFactor);
            }
        }
        speed = new Vector3(Mathf.Clamp(speed.x, -maxSpeedToPlayer, maxSpeedToPlayer), Mathf.Clamp(speed.y, -maxSpeedToPlayer, maxSpeedToPlayer), 0);

        transform.position += speed * Time.deltaTime;
    }
}