using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    static Camera mainCamera;
    public Camera assingedCamera;
    private Vector3 originalPos;

    private float shakeTimer;
    private float shakeAmount;

    public static float currendCredits = 0f;
    public static bool UsingGamepad = false;
    public static bool UsingUnityUI = true;

    void Start() {
        if (mainCamera == null) mainCamera = Camera.main;

        
    }

    void Update() {
        /*
        if (Input.GetButtonDown("Switch Weapon")) {
            StartCoroutine(ShakeCamera(0.2f, 0.05f));
            ScreenShake(0.25f, 0.25f);
        }
        */

        //Camera Shake 
        if (shakeTimer > 0) {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + ShakePos.x, mainCamera.transform.position.y + ShakePos.y, mainCamera.transform.position.z);

            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0) {
                Debug.Log("reset");
                mainCamera.transform.position = originalPos;
            }
        }

        //Update UI
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            float _MaxHealth = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().MaxHealth;
            float _currendHealth = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().currendHealth;
            PlayerControllerScript.Weapons _currentWeapon = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().currentWeapon;
            if (UsingUnityUI) {
                if (onlyOnGOwithTag("Health Bar UI")) {
                    GameObject.FindGameObjectsWithTag("Health Bar UI")[0].GetComponent<Slider>().maxValue = _MaxHealth;
                    GameObject.FindGameObjectsWithTag("Health Bar UI")[0].GetComponent<Slider>().value = _currendHealth;
                }
                if (onlyOnGOwithTag("Currend Weapon UI")) {
                    GameObject.FindGameObjectsWithTag("Currend Weapon UI")[0].GetComponent<Text>().text = "Weapon:" + System.Environment.NewLine + _currentWeapon.ToString();
                }
                if (onlyOnGOwithTag("Currend Credits UI")) {
                    GameObject.FindGameObjectsWithTag("Currend Credits UI")[0].GetComponent<Text>().text = "Credits:" + System.Environment.NewLine + currendCredits.ToString();
                }
                if (onlyOnGOwithTag("Cooldown UI")) {
                    
                    GameObject.FindGameObjectsWithTag("Cooldown UI")[0].GetComponent<Slider>().value = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControllerScript>().GetPercentUnitCooldown();
                }
            }
            else {
                if (onlyOnGOwithTag("Health Bar UI")) {
                    GameObject HealthBarUIGO = GameObject.FindGameObjectsWithTag("Health Bar UI")[0];
                    HealthBarUIGO.transform.localScale = new Vector3(_currendHealth / _MaxHealth, HealthBarUIGO.transform.localScale.y, HealthBarUIGO.transform.localScale.z);
                }
                /*
                if (onlyOnGOwithTag("Currend Weapon UI")) {
                    GameObject.FindGameObjectsWithTag("Currend Weapon UI")[0].transform = "Weapon:" + System.Environment.NewLine + _currentWeapon.ToString();
                }
                if (onlyOnGOwithTag("Currend Credits UI")) {
                    GameObject.FindGameObjectsWithTag("Currend Credits UI")[0].transform = "Credits:" + System.Environment.NewLine + currendCredits.ToString();
                }
                */
            }
        }
    }

    public static bool onlyOnGOwithTag(string _tag) {
        bool r = false;
        if (GameObject.FindGameObjectsWithTag(_tag).Length != 0) {
            if (GameObject.FindGameObjectsWithTag(_tag).Length > 1) {
                Debug.LogError("There are more than one " + _tag + " objects in the scene.");
            }
            else {
                r = true;
            }
        }
        else {
            Debug.LogError("There is no " + _tag + " in the scene.");
        }
        return r;
    }

    public void ScreenShake_old(float shakeStrength, float shakeDuration) {

        originalPos = mainCamera.transform.position;

        shakeAmount = shakeStrength;
        shakeTimer = shakeDuration;
    }

    public IEnumerator ShakeAssingedCamera(float duration, float magnitude) {
        Vector3 originalPosition = assingedCamera.transform.position;

        float timeElepsed = 0;

        while (timeElepsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            assingedCamera.transform.position = new Vector3(assingedCamera.transform.position.x + x, assingedCamera.transform.position.y + y, assingedCamera.transform.position.z);

            timeElepsed += Time.deltaTime;

            yield return null;
        }
        mainCamera.transform.position = originalPosition;
    }

    public static IEnumerator ShakeMainCamera(float duration, float magnitude) {
        Vector3 originalPosition = mainCamera.transform.position;

        float timeElepsed = 0;

        while (timeElepsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + x, mainCamera.transform.position.y + y, mainCamera.transform.position.z);

            timeElepsed += Time.deltaTime;

            yield return null;
        }
        mainCamera.transform.position = originalPosition;
    }

    /*
    public static void ShakeMainCameraStart(float _duration, float _magnitude) {

            //StartCoroutine(ShakeMainCamera(_duration, _magnitude));
    }
    */
}
