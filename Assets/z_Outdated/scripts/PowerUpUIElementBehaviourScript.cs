using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUIElementBehaviourScript : MonoBehaviour {

    public IEnumerator DestroyAfterTime(float _duration) {
        yield return new WaitForSecondsRealtime(_duration);
        Destroy(this.gameObject);
    }
}
