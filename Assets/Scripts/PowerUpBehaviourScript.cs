using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviourScript : MonoBehaviour {

    public Vector3 startDirection = new Vector2(1, 0);

    public enum PowerUpTypes { Invincibility, DamageUp }
    public PowerUpTypes currendPowerUpType;

    [SerializeField]
    private float TimeBeforeDestroy = 15f;
    [SerializeField]
    private float MinDistaceToPlayer = 2f;
    [SerializeField]
    private float MaxSpeedToPlayer = 1.5f;
    [SerializeField]
    float turnspeed = 10f;

    [SerializeField]
    private float accToPlayer = 1f;
    [SerializeField]
    private float accaccToPlayer = 0.1f;
    [SerializeField]
    private float deaccToPlayer = 0.05f;
    

    private Vector3 movehere;

    void Start() {
        StartCoroutine(destroyAfterTime());

        startDirection = transform.rotation * startDirection;
        transform.rotation = Quaternion.identity;

        switch (currendPowerUpType) {
            case (PowerUpTypes.Invincibility):
                //change apperance
                break;
            case (PowerUpTypes.DamageUp):
                //change apperance
                break;
            default:
                Debug.LogError("The PickUp -" + currendPowerUpType.ToString() + "- has no apperance assinged!");
                break;
        }
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


            movehere = GameObject.FindGameObjectWithTag("Player").transform.position;
            //movehere = Vector2.Lerp(movehere, GameObject.FindGameObjectWithTag("Player").transform.position, turnspeed * Time.deltaTime);
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
}
