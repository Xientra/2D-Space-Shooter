using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviourScript : LaserBulletBehaviourScript {

    public enum CameraShakePresets {
        _null_, Explosion_big, Explosion_small, Explosion_impact
    }
    public CameraShakePresets CameraShakePreset = CameraShakePresets._null_;

    protected override bool OnExplosion() {
        base.OnExplosion();

        if (CameraShakePreset == CameraShakePresets.Explosion_big) {
            StartCoroutine(GameControllerScript.ShakeMainCamera(0.2f, 0.1f));

            AudioControllerScript.activeInstance.PlaySound("ExplosionBig");
        }
        if (CameraShakePreset == CameraShakePresets.Explosion_small) {
            StartCoroutine(GameControllerScript.ShakeMainCamera(0.1f, 0.05f));

            AudioControllerScript.activeInstance.PlaySound("ExplosionSmall");
        }
        if (CameraShakePreset == CameraShakePresets.Explosion_impact) {
            StartCoroutine(GameControllerScript.ShakeMainCamera(0.9f, 0.5f, 0.975f));

            AudioControllerScript.activeInstance.PlaySound("ExplosionImpact");
        }

        return true; //this marks this method as called from ExplosionBehaviourScript
    }
}