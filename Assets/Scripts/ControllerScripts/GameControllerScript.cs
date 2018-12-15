using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Timers;

public class GameControllerScript : MonoBehaviour {

    static Camera mainCamera;
    public Camera assingedCamera;
    private Vector3 originalPos;

    public GameObject StarsGo;

    private float shakeTimer;
    private float shakeAmount;

    public static bool GameIsPaused = false;
    public static bool UsingGamepad = false;
    public bool UsingUnityUI = true;

    public GameObject toAssingFirstWep;
    public GameObject toAssingSecondWep;

    public static GameObject PlayerFirstWeapon;
    public static GameObject PlayerSecondWeapon;
    public static float currendCredits = 0f;
    public static float currendScore = 1000f;
    public static float HightScore = 0f;

    float scoreIntervall = 1000f; //1 Second?
    float scorePerTick = 3f;

    Timer scoreTimer = new Timer();

    void Start() {
        //Time.timeScale = 0.5f;

        //assing stuff if it's still null
        if (mainCamera == null) mainCamera = Camera.main;

        //inistikasdlize timer
        scoreTimer.Elapsed += new ElapsedEventHandler(OnScoreTimerTick);
        scoreTimer.Interval = scoreIntervall;
        scoreTimer.Start();

        currendScore = 0f;

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

        if (toAssingFirstWep != null) 
            PlayerFirstWeapon = toAssingFirstWep;
        if (toAssingSecondWep != null)
            PlayerSecondWeapon = toAssingSecondWep;
    }
    IEnumerator InstantiateStuffAfterOneFrame() {
        yield return null;
        if (SceneManager.GetActiveScene().name != "Main Menu") {
            if (GameObject.FindGameObjectWithTag("Player") == null) {
                Debug.Log("Spawned Player");
                Instantiate(ObjectHolder._PlayerShips[ObjectHolder.GetPlayerShipIndex(PlayerBehaviourScript.Ships.Standart)]);
            }
        }
        if (GameObject.FindGameObjectWithTag("Stars") == null) {
            Debug.Log("Instantiated Stars");
            Instantiate(StarsGo);
        }
    }

    void Update() {

        if (Time.timeScale == 0 || SceneManager.GetActiveScene().name == "Main Menu") {
            scoreTimer.Stop();
        }

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

    private void OnScoreTimerTick(object source, ElapsedEventArgs e) {
        //Debug.Log(scoreTimer.Enabled);
        GameControllerScript.currendScore += scorePerTick;
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
        
        while (timeElepsed < duration && GameIsPaused == false) {
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

        while (timeElepsed < duration && GameIsPaused == false) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + x, mainCamera.transform.position.y + y, mainCamera.transform.position.z);

            timeElepsed += Time.deltaTime;

            yield return null;
        }
        mainCamera.transform.position = originalPosition;
    }
    public static IEnumerator ShakeMainCamera(float duration, float magnitude, float MagnitudeMultiplyer) {
        Vector3 originalPosition = mainCamera.transform.position;

        float timeElepsed = 0;

        while (timeElepsed < duration && GameIsPaused == false) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, mainCamera.transform.position.z);

            magnitude *= MagnitudeMultiplyer;

            timeElepsed += Time.deltaTime;

            yield return null;
        }
        mainCamera.transform.position = originalPosition;
    }

    public static void PauseGame(bool _state) {
        if (_state == true) {
            Time.timeScale = 0;
            GameIsPaused = true;
        }
        else if (_state == false) {
            Time.timeScale = 1;
            GameIsPaused = false;
        }
    }

    public void StartGameOver(float animationLegth) {

        scoreTimer.Stop();
        if (scoreTimer != null)
        Debug.Log(scoreTimer.Enabled);

        if (currendScore > HightScore)
            HightScore = currendScore;
        //currendScore = 0f;

        StartCoroutine(DelayGameOver(animationLegth));
    }

    public IEnumerator DelayGameOver(float _delay) {


        yield return new WaitForSecondsRealtime(_delay * 0.6f);

        for (int i = 1; i <= 10; i++) {
            yield return new WaitForSecondsRealtime(0.16f);
            if (Time.timeScale - 0.1f < 0)
                Time.timeScale = 0;
            else
                Time.timeScale -= 0.1f;
        }

        yield return new WaitForSecondsRealtime(_delay * 0.2f);

        if (GameObject.FindGameObjectWithTag("InGameUI") != null)
            GameObject.FindGameObjectWithTag("InGameUI").GetComponent<InGameUIControllerScript>().OpenInGameDeathMenu();
        else Debug.LogError("The (Player/GameController) Object could not find Go with Tag: \"InGameUI\" and so not open the InGameDeathMenu");
    }
}