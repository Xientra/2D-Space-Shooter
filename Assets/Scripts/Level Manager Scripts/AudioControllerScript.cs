using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioControllerScript : MonoBehaviour {

    public static AudioControllerScript activeInstance;

    public AudioClip[] sounds;
    private AudioSource[] audioSources;

    void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this.gameObject);
            return;
        }

        
        audioSources = new AudioSource[sounds.Length];

        for (int i = 0; i < sounds.Length; i++) {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = sounds[i];
        }
    }

    void Start() {
    }

    void Update() {
    }

    public void PlaySound(string _soundName) {
        if (GameControllerScript.SoundIsMuted == false) {
            
            GetAudioSource(_soundName).Play();

        }
    }

    private AudioSource GetAudioSource(string _soundName) {
        foreach (AudioSource AuSo in audioSources) {
            if (AuSo.clip.name == _soundName)
                return AuSo;
        }
        Debug.LogError("There is no AudioSource for the Sound with the Name: \"" + _soundName + "\".");
        return null;
    }

    //not used anywhere 
    private AudioClip GetSound(string _soundName) {
        foreach (AudioClip ac in sounds) {
            if (ac.name == _soundName)
                return ac;
        }
        Debug.LogError("There is no Sound with the Name: \"" + _soundName + "\".");
        return null;
    }
}