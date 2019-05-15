using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FireWorksSoundScript : MonoBehaviour {

    public ParticleSystem FireWorkParticleSystem;
    private int lastNumberOfParticles;

    void Start() {
        FireWorkParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update() {
        int count = FireWorkParticleSystem.particleCount;

        if (count < lastNumberOfParticles) { //a particle has died
            AudioControllerScript.activeInstance.PlaySound("FireWorkExplosion");
        }
        else if (count > lastNumberOfParticles) { //a particle has been born
            AudioControllerScript.activeInstance.PlaySound("FireWorkStart");
        }

        lastNumberOfParticles = count;
    }
}