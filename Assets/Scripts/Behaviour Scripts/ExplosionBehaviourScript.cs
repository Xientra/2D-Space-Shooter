using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviourScript : LaserBulletBehaviourScript {

    public enum ExplosionTypes {
        _null_, Explosion_big, Explosion_small, Explosion_impact
    }
    public ExplosionTypes explosionType = ExplosionTypes._null_;

    protected override void OnExplosion() {
        base.OnExplosion();

        if (explosionType == ExplosionTypes.Explosion_big) {
            StartCoroutine(GameControllerScript.ShakeMainCamera(0.2f, 0.1f));
        }
        if (explosionType == ExplosionTypes.Explosion_small) {
            StartCoroutine(GameControllerScript.ShakeMainCamera(0.1f, 0.05f));
        }
        if (explosionType == ExplosionTypes.Explosion_impact) {
            StartCoroutine(GameControllerScript.ShakeMainCamera(0.9f, 0.5f, 0.975f));
        }
    }

}
