using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {

    static Camera mainCamera;
    public Camera assingedCamera;
    private Vector3 originalPos;

    public GameObject StarsGo;

    private float shakeTimer;
    private float shakeAmount;


    public static bool UsingGamepad = false;
    public bool UsingUnityUI = true;

    public static GameObject PlayerFirstWeapon;
    public static GameObject PlayerSecondWeapon;
    public static float currendCredits = 0f;

    void Start() {
        //assing stuff if it's still null
        if (mainCamera == null) mainCamera = Camera.main;

        StartCoroutine(DoStuffAfterOneFrame());
        StartCoroutine(InstantiateStuffAfterOneFrame());
    }
    IEnumerator DoStuffAfterOneFrame() {
        yield return 0;
        if (MainMenuControllerScript.firstWeaponGO != null)
            PlayerFirstWeapon = MainMenuControllerScript.firstWeaponGO;
        else
            PlayerFirstWeapon = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Standart_lvl_1)];

        if (MainMenuControllerScript.secondWeaponGO != null)
            PlayerSecondWeapon = MainMenuControllerScript.secondWeaponGO;
        else
            PlayerSecondWeapon = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1)];
    }

    IEnumerator InstantiateStuffAfterOneFrame() {
        yield return null;

        if (GameObject.FindGameObjectWithTag("Player") == null) {
            Debug.Log("Spawned Player");
            Instantiate(ObjectHolder._PlayerShips[ObjectHolder.GetPlayerShipIndex(PlayerBehaviourScript.Ships.Standart)]);
        }
        if (GameObject.FindGameObjectWithTag("Stars") == null) {
            Debug.Log("Instantiated Stars");
            Instantiate(StarsGo);
        }
    }

    void Update() {

        //Camera Shake 
        if (shakeTimer > 0) {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + ShakePos.x, mainCamera.transform.position.y + ShakePos.y, mainCamera.transform.position.z);

            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0) {
                //Debug.Log("reset");
                mainCamera.transform.position = originalPos;
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
}