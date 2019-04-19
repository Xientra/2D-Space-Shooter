using System;
using System.Timers;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {

    public static GameControllerScript instance;


    static Camera mainCamera;
    public Camera assingedCamera;
    private Vector3 originalPos;

    public GameObject StarsGo;
    [HideInInspector]
    public GameObject StarsInstance;

    public GameObject CursorGUIGo;
    [HideInInspector]
    public GameObject CursorGUIInstance;

    [HideInInspector]
    public GameObject PlayerInstance;

    [Header("Only to Test Weapons")]
    public GameObject toAssingFirstWep;
    public GameObject toAssingSecondWep;



    /*==========All Static Variables==========*/
    private static bool doOnce = true;

    public static bool[] LevelProgress = { false, false, false };

    public static float currendCredits = 300f;
    public static float currendScore = 0f;
    public static float HightScore = 0f;

    public static GameObject PlayerFirstWeapon;
    public static GameObject PlayerSecondWeapon;
    
    public static bool GameIsPaused = false;
    public static bool SoundIsMuted = false;
    public static bool MusicIsMuted = false;
    public static bool UsingGamepad = false;
    


    /*==========Timer==========*/

    private float shakeTimer;
    private float shakeAmount;


    float scoreIntervall = 1000f; //1 Second?
    float scorePerTick = 3f;

    Timer scoreTimer = new Timer();



    private void Awake() {
        if (instance != null) {
            Debug.LogError("There is more than one GameController in the scene");
        }
        else {
            instance = this;
        }    
    }

    void Start() {
        LoadGame();

        //assing stuff if it's still null
        if (mainCamera == null) mainCamera = Camera.main;

        //inistikasdlize timer
        scoreTimer.Elapsed += new ElapsedEventHandler(OnScoreTimerTick);
        scoreTimer.Interval = scoreIntervall;
        scoreTimer.Start();

        currendScore = 0f;

        //Instantiate stuff based on the scene
        bool b = false;
        foreach (Transform t in transform) {
            if (t.CompareTag("Stars")) {
                b = true;
            }
        }
        if (b == false) {
            StarsInstance = Instantiate(StarsGo, transform);
        }

        if (SceneManager.GetActiveScene().name != "Main Menu") {
            if (GameObject.FindGameObjectWithTag("Player") == null) {
                Debug.Log("Spawned Player");
                PlayerInstance = Instantiate(ObjectHolder._PlayerShips[ObjectHolder.GetPlayerShipIndex(PlayerBehaviourScript.Ships.Standart)]);

                if (CursorGUIGo != null) {
                    CursorGUIInstance = Instantiate(CursorGUIGo, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);
                    Cursor.visible = false;
                }
            }
        }
        else {
            Cursor.visible = true;
        }

        if (doOnce == true) {
            doOnce = false;
            Debug.Log("doing once");

            //currendCredits += 10000f;

            //Sets all Weapons.isBought to false exept for a few selected ones
            foreach (GameObject playerWeapon in ObjectHolder._PlayerWeapons) {
                switch (playerWeapon.GetComponent<WeaponBehaviourScript>().WeaponType) {
                    case WeaponBehaviourScript.WeaponTypes.Standart_lvl_1:
                        playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = true;
                        break;
                    case WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1:
                        playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = true;
                        break;

                    default:
                        playerWeapon.GetComponent<WeaponBehaviourScript>().isBought = false;
                        break;
                }
            }
        }

        //Assinging Player Weapons if needed
        if (PlayerFirstWeapon == null)
            PlayerFirstWeapon = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Standart_lvl_1)];
        if (PlayerSecondWeapon == null)
            PlayerSecondWeapon = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex(WeaponBehaviourScript.WeaponTypes.Shotgun_lvl_1)];


        //Only for testing weapons
        if (toAssingFirstWep != null)
            PlayerFirstWeapon = toAssingFirstWep;
        if (toAssingSecondWep != null)
            PlayerSecondWeapon = toAssingSecondWep;
    }

    void Update() {

        if (Time.timeScale == 0 || SceneManager.GetActiveScene().name == "Main Menu") {
            scoreTimer.Stop();
        }

        //Camera Shake 
        if (shakeTimer > 0) {
            Vector2 ShakePos = UnityEngine.Random.insideUnitCircle * shakeAmount;

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
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

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
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

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
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

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
        //Debug.Log(scoreTimer.Enabled);

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

        if (currendScore > HightScore)
            HightScore = currendScore;
        //currendScore = 0f;

        SaveGame();

        if (GameObject.FindGameObjectWithTag("InGameUI") != null)
            GameObject.FindGameObjectWithTag("InGameUI").GetComponent<InGameUIControllerScript>().OpenInGameDeathMenu();
        else Debug.LogError("The (Player/GameController) Object could not find Go with Tag: \"InGameUI\" and so not open the InGameDeathMenu");
    }




    public void SaveGame() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");

        PlayerData data = new PlayerData();

        data.doOnce = doOnce;
        data.currendCredits = currendCredits;
        data.HightScore = HightScore;
        data.LevelProgress = LevelProgress;
        data.playerFirstWeaponInt = (int)PlayerFirstWeapon.GetComponent<WeaponBehaviourScript>().WeaponType;
        data.playerSecondWeaponInt = (int)PlayerSecondWeapon.GetComponent<WeaponBehaviourScript>().WeaponType;
        data.NewWeaponPrice = MainMenuControllerScript.NewWeaponPrice;

        bool[] _unlockedWeapons = new bool[ObjectHolder._PlayerWeapons.Length];

        for (int i = 0; i < ObjectHolder._PlayerWeapons.Length; i++) {
             _unlockedWeapons[i] = ObjectHolder._PlayerWeapons[i].GetComponent<WeaponBehaviourScript>().isBought;
        }
        data.UnlockedWeapons = _unlockedWeapons;


        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game was saved");
        //Instantiate(ObjectHolder._Effects[ObjectHolder.GetEffectIndex(EffectBehaviourScript.EffectTypes.SavingIcon)], new Vector3(17, 10), Quaternion.identity); //mainCamera.GetComponent<Camera>().orthographicSize
    }

    public void LoadGame() {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open); 

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            doOnce = data.doOnce;
            currendCredits = data.currendCredits;
            HightScore = data.HightScore;
            LevelProgress = data.LevelProgress;
            PlayerFirstWeapon = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex((WeaponBehaviourScript.WeaponTypes)data.playerFirstWeaponInt)];
            PlayerSecondWeapon = ObjectHolder._PlayerWeapons[ObjectHolder.GetPlayerWeaponIndex((WeaponBehaviourScript.WeaponTypes)data.playerSecondWeaponInt)];

            MainMenuControllerScript.NewWeaponPrice = data.NewWeaponPrice;

            bool[] _unlockedWeapons = data.UnlockedWeapons;

            for (int i = 0; i < ObjectHolder._PlayerWeapons.Length; i++) {
                ObjectHolder._PlayerWeapons[i].GetComponent<WeaponBehaviourScript>().isBought = _unlockedWeapons[i];
            }
        }

        Debug.Log("Game was loaded");
    }

    private void OnDisable() {
        SaveGame();
    }
}