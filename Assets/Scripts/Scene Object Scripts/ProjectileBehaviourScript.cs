using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviourScript : MonoBehaviour {

    private LineRenderer lineRenderer;

    public GameObject createOnDeath;
    public GameObject explosionOnDeath;

    public Vector3 direction = new Vector3(0, 1, 0);


    /*--------------------Affiliation--------------------*/

    public enum BulletTypes {
        _null,
        BlasterShoot_lvl_1, BlasterShoot_lvl_2, BlasterShoot_lvl_3,
        ShotgunBullet_lvl_1, ShotgunBullet_lvl_2, ShotgunBullet_lvl_3,
        ChainGunBullet_lvl_1, ChainGunBullet_lvl_2, ChainGunBullet_lvl_3,
        HomingBullet_lvl_1, HomingBullet_lvl_2, HomingBullet_lvl_3,
        HelixBullet_lvl_1, HelixBullet_lvl_2, HelixBullet_lvl_3, HelixBulletChild,
        LaserSword_lvl_1, LaserSword_lvl_2, LaserSword_lvl_3,
        Missile_lvl_1, Missile_lvl_2, Missile_lvl_3,
        Grenade_lvl_1, Grenade_lvl_2, Grenade_lvl_3,
        Shrapnel_lvl_1, Shrapnel_lvl_2, Shrapnel_lvl_3, ShrapnellBullet,
        HelixBullet_lvl_X, SniperBullet_lvl_X, WaveBullet_lvl_X, SplitBullet_lvl_X,
    }

    [Header("Affiliation: ")]

    public BulletTypes bulletType = BulletTypes._null;

    [SerializeField]
    private bool isEnemyBullet = false;

    [SerializeField]
    private bool isShootable = false;

    private Vector3 lastFramePosition;


    [Header("Stats: ")]
    /*--------------------Stats--------------------*/

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    public float damage = 10f;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private float DelayDestructionTime = 0.5f; //this should be at least as long as the camera is shaken on hit (0.2f)
    private bool DelayingDestruction = false;
    //private float cooldown = 0.5f;
    //private enum SpecialEffects { none };
    //[SerializeField]
    //private SpecialEffects SpecialEffect = SpecialEffects.none;


    private bool SelfDestructionActive = false;


    /*--------------------Player Bullet Specific Behaviour Variables--------------------*/
    [Header("Player Bullet Specific Behaviour Variables: ")]

    //-----homing-----
    [SerializeField]
    private float homingStrength = 0f;
    [SerializeField]
    private float homingDistance = 10f;
    //-----Rocked homing-----
    [SerializeField]
    private float RotationSpeed = 0.1f;
    private float RotationProgress = 0f;
    //LaserSword-----
    [SerializeField]
    private float LaserSwordAccellerationSpeed = 0.1f;
    private float TempSpeed;
    private float LaserSwordAcc = 0f;
    //-----Helix-----
    private float HelixBulletChild_RotationSpeed = 10f;
    //-----Shrapnell-----
    [SerializeField]
    private int AmountOfShrapnellBulletsSpawnedOnDeath = 0;
    //-----Shrapnell Bullet-----
    private float shrapnellBulletRNGSpread = 24;


    /*--------------------Additional Bullet Behaviour Variables--------------------*/
    public enum AdditionalBulletBehaviour {
        _null_, LightningMovement, SquareSplitAtSomePoint, CreateBulletsOnTheSide //NormalSplitAtSomePoint?
    }

    [Header("Additional Bullet Behaviour Variables: ")]

    public AdditionalBulletBehaviour additionalBulletBehaviour = AdditionalBulletBehaviour._null_;
    //lightning movement
    private const float changeAngleLiklyhood = 0.05f;
    private float AngleDeviation = 80f;



    /*--------------------PowerUp Variables--------------------*/
    public static float damageMultiplyer = 1f;

    private void Start() {
        StartPlayerBulletSpecificBehaviour();
        StartAdditionalBulletBehaviour();

        lastFramePosition = transform.position;
        StartCoroutine(destroyAfterTime());
    }

    void Update() {
        PerformPlayerBulletSpecificBehaviour();
        PerformAdditionalBulletBehaviour();

        if (SelfDestructionActive == false) {
            CollisionDetectionToLastPosition();
        }
        lastFramePosition = transform.position;
    }

    void FixedUpdate() {
        if (Time.timeScale > 0) {
            Movement();
            HomingMovement();
        }
    }

    private void Movement() {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void HomingMovement() {
        if (homingStrength != 0) {
            GameObject nearEnemy = GetNearestEnemyInRadius(homingDistance);
            if (nearEnemy != null) {
                if (Vector3.SqrMagnitude(nearEnemy.transform.position - this.transform.position) <= homingDistance) {
                    Vector3 PlayerDirection = Vector3.Normalize(nearEnemy.transform.position - transform.position);

                    direction += PlayerDirection * homingStrength;
                    transform.position += direction * Time.deltaTime;
                }
            }
        }
    }

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(duration);
        if (SelfDestructionActive != true) {
            //Debug.Log("SelfDes1");
            InitiliseSelfDestruction();
        }
    }

    void CollisionDetectionToLastPosition() {

        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, -transform.position + lastFramePosition, (-transform.position + lastFramePosition).magnitude);

        Debug.DrawLine(transform.position, lastFramePosition);

        foreach (RaycastHit2D rH in raycastHits) {
            OnTriggerEnter2D(rH.collider);
        }
    }

    private void OnDestroy() {
        if (additionalBulletBehaviour == AdditionalBulletBehaviour.LightningMovement) {
            CancelInvoke("PerformLightingMovement");
        }
    }

    public void InitiliseSelfDestruction() {
        SelfDestructionActive = true;

        if (this.GetType() == typeof(ExplosionBehaviourScript)) {
            OnExplosion();
            StartCoroutine(DelayDestruction());
        }
        else { //================================= Normal Destruction =================================
            //specific bullet behaviour
            if (AmountOfShrapnellBulletsSpawnedOnDeath != 0) {
                for (int i = 1; i <= AmountOfShrapnellBulletsSpawnedOnDeath; i++) {
                    Instantiate(ObjectHolder._Bullets[ObjectHolder.GetBulletIndex(BulletTypes.ShrapnellBullet)], transform.position, Quaternion.Euler(0, 0, (360 / AmountOfShrapnellBulletsSpawnedOnDeath) * i)); //360 / Amount devides the cicle to all the shrapnellbullets
                }
            }

            //detaches all (Helix)BulletChilds of the own transform and gives them speed
            foreach (Transform ImminentChildTransform in transform) {
                if (ImminentChildTransform.GetComponent<ProjectileBehaviourScript>() != null) {
                    ImminentChildTransform.rotation = transform.rotation;
                    ImminentChildTransform.GetComponent<ProjectileBehaviourScript>().speed = speed / 2;
                    ImminentChildTransform.parent = null;
                }
            }

            foreach (LineRenderer LineRendererGo in GetComponentsInChildren<LineRenderer>()) {
                Destroy(LineRendererGo.transform.gameObject);
            }

            //deactivate Collider
            if (GetComponent<CircleCollider2D>() != null) {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            if (GetComponent<BoxCollider2D>() != null) {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            if (GetComponent<CapsuleCollider2D>() != null) {
                GetComponent<CapsuleCollider2D>().enabled = false;
            }

            StartCoroutine(DelayDestruction());

            speed = speed / 10f;

            if (createOnDeath != null) {
                Instantiate(createOnDeath, transform);

                if (createOnDeath.GetComponent<ProjectileBehaviourScript>() != null)
                    Debug.LogWarning("If you want to Intantiate an explosion please do that with explosionOnDeath.");
            }
            if (explosionOnDeath != null) {
                Instantiate(explosionOnDeath, transform.position, Quaternion.identity);

                if (explosionOnDeath.GetComponent<ProjectileBehaviourScript>() == null)
                    Debug.LogWarning("If you want to Intantiate an OnDeathEffect please do that with createOnDeath.");
            }
        }

        StartCoroutine(FinishDestructionAfterOneFrame());
    }

    IEnumerator DelayDestruction() {
        DelayingDestruction = true;
        yield return new WaitForSeconds(DelayDestructionTime);

        Destroy(this.gameObject);
        DelayingDestruction = false;
    }

    IEnumerator FinishDestructionAfterOneFrame() {
        yield return 0;

        foreach (SpriteRenderer SR in GetComponentsInChildren<SpriteRenderer>()) {
            if (SR.gameObject.GetComponent<ProjectileBehaviourScript>() != null) {
                SR.enabled = false;
            }
        }

        if (this.GetType() != typeof(ExplosionBehaviourScript)) {
            //Unparent the TrailRendererGo or the ParticleTrailGo
            foreach (TrailRenderer TR in GetComponentsInChildren<TrailRenderer>()) {
                if (TR.gameObject.GetComponent<ProjectileBehaviourScript>() == null) {
                    TR.time = TR.time / 2;
                    TR.transform.SetParent(null, true);
                }
            }
            foreach (ParticleSystem Ps in GetComponentsInChildren<ParticleSystem>()) {
                if (Ps.gameObject.GetComponent<ProjectileBehaviourScript>() == null) {
                    Ps.Stop();
                }
            }
        }

        if (DelayingDestruction == false) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (this.GetType() != typeof(ExplosionBehaviourScript)) {
            if (collision.gameObject.layer == 8/*Static*/) {
                if (collision.CompareTag("BulletLimiter")) {
                    Destroy(this.gameObject);
                }
                else
                    InitiliseSelfDestruction();
            }

            if (isEnemyBullet == false) { //if bullet from player
                if (collision.gameObject.layer == 10/*Enemy*/) {
                    if (collision.gameObject.GetComponent<EnemyBehaviourScript>().Health >= 0) {
                        collision.gameObject.GetComponent<EnemyBehaviourScript>().Health -= damage * damageMultiplyer;

                        if (bulletType == BulletTypes.SniperBullet_lvl_X != true) {
                            InitiliseSelfDestruction();
                        }
                    }
                }
            }
            else { //if bullet is from enemy
                if (collision.gameObject.CompareTag("Player")) {
                    collision.gameObject.GetComponent<PlayerBehaviourScript>().ChangeHealthBy(-damage);
                    StartCoroutine(GameControllerScript.ShakeMainCamera(0.2f, 0.05f));
                    InitiliseSelfDestruction();
                }
            }

            if (collision.gameObject.CompareTag("Projectile")) {
                if (this.isShootable == true) {
                    if (collision.gameObject.GetComponent<ProjectileBehaviourScript>().isEnemyBullet != true) {
                        InitiliseSelfDestruction();
                    }
                }
                if (bulletType == BulletTypes.WaveBullet_lvl_X) {
                    if (collision.gameObject.GetComponent<ProjectileBehaviourScript>().isEnemyBullet == true) {
                        //collision.gameObject.GetComponent<ProjectileBehaviourScript>().InitiliseSelfDestruction();
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
        else { //this is an exposion
            if (isEnemyBullet == false) { // is player explosion
                if (collision.gameObject.layer == 10/*Enemy*/) {
                    collision.gameObject.GetComponent<EnemyBehaviourScript>().Health -= damage;

                }
            }
            else { //is enemy explosion
                if (collision.gameObject.CompareTag("Player")) {
                    collision.gameObject.GetComponent<PlayerBehaviourScript>().ChangeHealthBy(-damage);

                    InitiliseSelfDestruction();
                }
            }
        }
    }

    [System.Obsolete("Fuck no; use GetNearestEnemyInRadius instead.")]
    GameObject GetNearestEnemy() {
        GameObject returnGo = null;
        GameObject[] GOList = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject GO in GOList) {
            if (GO.layer == 10 /*Enemy*/) {
                if (returnGo == null) {
                    returnGo = GO;
                }
                else {
                    if (Vector3.SqrMagnitude(GO.transform.position - this.transform.position) < Vector3.SqrMagnitude(returnGo.transform.position - this.transform.position)) {
                        returnGo = GO;
                    }
                }
            }
        }
        return returnGo;
    }

    GameObject GetNearestEnemyInRadius(float _radius) {
        GameObject returnGo = null;
        float returnGoDistance = 0;
        RaycastHit2D[] HitsInRadiusList = Physics2D.CircleCastAll(transform.position, _radius, Vector3.zero);

        //the HitsInRadiusList should allready be sorted from unity so the following should work but it didn't idk why 
        //int i = 0;
        //while (i < HitsInRadiusList.Length && HitsInRadiusList[i].collider.gameObject.layer != 10) {
        //    i++;
        //}
        //if (i < HitsInRadiusList.Length) {
        //    returnGo = HitsInRadiusList[i].collider.gameObject;
        //}
      
        foreach (RaycastHit2D Hit in HitsInRadiusList) {
            if (Hit.collider.gameObject.layer == 10 /*Enemy*/) {
                if (returnGo == null) {
                    returnGo = Hit.collider.gameObject;
                    returnGoDistance = Vector3.SqrMagnitude(returnGo.transform.position - this.transform.position);
                }
                else {
                    if (Vector3.SqrMagnitude(Hit.collider.transform.position - this.transform.position) < returnGoDistance) {
                        returnGo = Hit.collider.gameObject;
                        returnGoDistance = Vector3.SqrMagnitude(returnGo.transform.position - this.transform.position);
                    }
                }
            }
        }
        
        return returnGo;
    }

    protected virtual bool OnExplosion() {
        return false;
    }

    private void StartPlayerBulletSpecificBehaviour() {
        switch (bulletType) {
            case (BulletTypes.HelixBulletChild):
                duration = transform.parent.gameObject.GetComponent<ProjectileBehaviourScript>().duration;

                switch (transform.parent.gameObject.GetComponent<ProjectileBehaviourScript>().bulletType) {
                    case (BulletTypes.HelixBullet_lvl_1):
                        HelixBulletChild_RotationSpeed = 800f;
                        break;
                    case (BulletTypes.HelixBullet_lvl_2):
                        HelixBulletChild_RotationSpeed = 800f;
                        break;
                    case (BulletTypes.HelixBullet_lvl_3):
                        HelixBulletChild_RotationSpeed = 400f;
                        break;
                    case (BulletTypes.HelixBulletChild):
                        HelixBulletChild_RotationSpeed = 200f;
                        break;
                }
                break;
            case (BulletTypes.LaserSword_lvl_1):
            case (BulletTypes.LaserSword_lvl_2):
            case (BulletTypes.LaserSword_lvl_3):
                TempSpeed = speed;
                speed = 0;
                break;

            case (BulletTypes.HomingBullet_lvl_1):
            case (BulletTypes.HomingBullet_lvl_2):
            case (BulletTypes.HomingBullet_lvl_3):
                direction = transform.rotation * direction;
                transform.rotation = Quaternion.identity;
                break;

            case (BulletTypes.ShrapnellBullet):
                transform.rotation = transform.rotation * Quaternion.Euler(0, 0, Random.Range(shrapnellBulletRNGSpread, -shrapnellBulletRNGSpread));
                break;

            //this was ment for the precise grenade explosion(it explodes there where the mouse is)
            //case (BulletTypes.Grenade_lvl_2):
            //    Vector3 distance = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - transform.position;
            //    break;
        }
    }

    private void StartAdditionalBulletBehaviour() {
        switch (additionalBulletBehaviour) {
            case (AdditionalBulletBehaviour.LightningMovement):
                InvokeRepeating("PerformLightingMovement", 0f, 0.01f);
                break;
            case (AdditionalBulletBehaviour.SquareSplitAtSomePoint):
                StartCoroutine(SquareSplitAfterTime(duration / 4, 0.5f));
                break;
        }
    }


    private void PerformPlayerBulletSpecificBehaviour() {
        if (bulletType == BulletTypes.HelixBulletChild) {
            if (transform.parent != null)
                this.transform.RotateAround(transform.parent.transform.position, Vector3.forward, HelixBulletChild_RotationSpeed * Time.deltaTime);
        }

        if (bulletType == BulletTypes.LaserSword_lvl_1) {
            if (SelfDestructionActive == false) {
                if (speed <= TempSpeed) {
                    LaserSwordAcc += TempSpeed * Time.deltaTime * LaserSwordAccellerationSpeed;
                    speed += LaserSwordAcc;
                }
            }
        }

        if (bulletType == BulletTypes.Missile_lvl_1 || bulletType == BulletTypes.Missile_lvl_2 || bulletType == BulletTypes.Missile_lvl_3) {

            GameObject nearestEnemy = GetNearestEnemyInRadius(homingDistance);

            if (nearestEnemy != null) {
                if (Vector3.SqrMagnitude(nearestEnemy.transform.position - this.transform.position) <= homingDistance) {
                    //Draw Line Effect
                    if (GetComponentInChildren<LineRenderer>() != null) {
                        LineRenderer lr = GetComponentInChildren<LineRenderer>();

                        lr.SetPosition(0, lr.transform.position);
                        lr.SetPosition(1, nearestEnemy.transform.position);
                    }

                    //Homing Effect
                    RotationProgress += Time.deltaTime * RotationSpeed;
                    transform.up = Vector3.Lerp(transform.up, Vector3.Normalize(nearestEnemy.transform.position - transform.position), RotationProgress);
                }
                else {
                    if (GetComponentInChildren<LineRenderer>() != null) {
                        LineRenderer lr = GetComponentInChildren<LineRenderer>();
                        lr.SetPosition(0, lr.transform.position);
                        lr.SetPosition(1, lr.transform.position);
                    }
                }
            }
            else {
                if (GetComponentInChildren<LineRenderer>() != null) {
                    LineRenderer lr = GetComponentInChildren<LineRenderer>();
                    lr.SetPosition(0, lr.transform.position);
                    lr.SetPosition(1, lr.transform.position);
                }
            }
        }

        if ((bulletType == BulletTypes.Grenade_lvl_2 || bulletType == BulletTypes.Grenade_lvl_3) && Input.GetMouseButtonUp(0) == true) {
            if (SelfDestructionActive != true) {
                InitiliseSelfDestruction();
            }
        }
    }

    private void PerformAdditionalBulletBehaviour() {

    }

    private void PerformLightingMovement() {
        if (Random.Range(0f, 1f) <= changeAngleLiklyhood) {
            Quaternion rngRot = Quaternion.Euler(0, 0, Random.Range(AngleDeviation, -AngleDeviation));
            this.transform.rotation *= rngRot;
            StartCoroutine(LightingMovementRotateBack(rngRot));
        }
    }
    private IEnumerator LightingMovementRotateBack(Quaternion _quat) {
        float _time = Random.Range(0.01f, 0.15f);
        yield return new WaitForSeconds(_time);
        this.transform.rotation *= Quaternion.Inverse(_quat);
    }

    private IEnumerator SquareSplitAfterTime(float _time, float _distance) {
        yield return new WaitForSeconds(_time);

        GameObject go1 = Instantiate(gameObject, transform.position, transform.rotation);
        GameObject go2 = Instantiate(gameObject, transform.position, transform.rotation);

        ProjectileBehaviourScript bullet1 = go1.GetComponent<ProjectileBehaviourScript>();
        ProjectileBehaviourScript bullet2 = go2.GetComponent<ProjectileBehaviourScript>();

        bullet1.additionalBulletBehaviour = AdditionalBulletBehaviour._null_;
        bullet2.additionalBulletBehaviour = AdditionalBulletBehaviour._null_;

        //go1.transform.position += transform.right * _distance;
        //go2.transform.position -= transform.right * _distance;

        bullet1.direction.x = bullet1.direction.y / 5;
        bullet2.direction.x = -bullet2.direction.y / 5;

        //StartCoroutine(Test(go1, go2, _distance));



        bullet1.PerformSetDirectionAfterTime(direction, 0.5f);
        bullet2.PerformSetDirectionAfterTime(direction, 0.5f);

        //Destroy(this.gameObject);
    }

    private IEnumerator Test(GameObject _go1, GameObject _go2, float _distance) {
        yield return null;
        _go1.transform.position += transform.right * _distance;
        _go2.transform.position -= transform.right * _distance;
    }


    private void PerformSetDirectionAfterTime(Vector3 _direction, float _time){
        StartCoroutine(SetDirectionAfterTime(_direction, _time));
    }

    private IEnumerator SetDirectionAfterTime(Vector3 _direction, float _time) {

        yield return new WaitForSeconds(_time);

        direction = _direction;
    }
}