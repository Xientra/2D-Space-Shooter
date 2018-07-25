using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsHolder : MonoBehaviour {

    public GameObject[] Effects;
    public enum EffectNames { LaserLoaded }
    public EffectNames effectName;

    /*
	void Start () {
		
	}
	
	void Update () {
		
	}
    */
    public int GetEffectIndex(EffectNames _effectType) {
        int i = 0;
        foreach (GameObject go in Effects) {
            if (go != null) {
                if (effectName == _effectType) {
                    return i;
                }
            }
            i++;
        }
        Debug.LogError("Could not find: " + _effectType);
        return -1;
    }
}
