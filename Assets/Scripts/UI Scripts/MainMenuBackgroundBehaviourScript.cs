using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackgroundBehaviourScript : MonoBehaviour {

    public RuntimeAnimatorController AnimationToPlay;

    public GameObject[] SomeEnemyObjects;
    private GameObject SpawnedEnemyObject;

    private float distance_x;
    private float distance_y;
    private float offset;

    public float SpawnCooldownMin = 5f;
    public float SpawnCooldownMax = 6f;

    private float SpawnCooldown;
    private float SpawnCooldownTimeStamp;

    void Start () {
        // x / 9 * 16
        distance_x = Camera.main.orthographicSize / 9 * 16;
        distance_y = Camera.main.orthographicSize;
        offset = Camera.main.orthographicSize / 5;

        SpawnCooldown = Random.Range(SpawnCooldownMin, SpawnCooldownMax);
        //transform.localScale = new Vector2(2 * Camera.main.orthographicSize / 9 * 16, 2 * Camera.main.orthographicSize);
    }
	
	void Update () {
        if (SpawnCooldownTimeStamp <= Time.time) {
            if (SpawnedEnemyObject != null) {
                Destroy(SpawnedEnemyObject);
            }

            int rng = Random.Range(0, SomeEnemyObjects.Length);
            Debug.Log(rng);
            while (SomeEnemyObjects[rng] == null) {
                rng = Random.Range(0, SomeEnemyObjects.Length);
                Debug.Log(rng);
            }

            SpawnedEnemyObject = Instantiate(SomeEnemyObjects[rng], new Vector3(Random.Range(-(distance_x - offset), distance_x - offset), distance_y + offset), Quaternion.identity);

            SpawnedEnemyObject.GetComponentInChildren<Animator>().runtimeAnimatorController = AnimationToPlay;
            SpawnedEnemyObject.GetComponentInChildren<EnemyBehaviourScript>().AnimationStartDelay = 0f;
            SpawnedEnemyObject.GetComponent<EnemyParentBehaviourScript>().HealthBarObject.SetActive(false);


            SpawnCooldown = Random.Range(SpawnCooldownMin, SpawnCooldownMax);
            SpawnCooldownTimeStamp = Time.time + SpawnCooldown;
        }
    }
}