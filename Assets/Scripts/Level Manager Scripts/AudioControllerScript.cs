using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioControllerScript : MonoBehaviour {

    public static AudioControllerScript activeInstance;


    public SoundData[] soundData;

    public AudioClip[] sounds;

    private List<AudioSource> audioSourcesList = new List<AudioSource>();

    public bool mute = false;

    public bool useV2 = false;

    void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this.gameObject);
            return;
        }


        foreach (SoundData sD in soundData) {
            sD.audioSource = gameObject.AddComponent<AudioSource>();
            sD.audioSource.clip = sD.SoundClip;
            sD.audioSource.volume = sD.volume;
            sD.audioSource.pitch = sD.pitch;
            sD.audioSource.loop = sD.loop;
        }

        /*
        audioSources = new AudioSource[sounds.Length];

        for (int i = 0; i < sounds.Length; i++) {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = sounds[i];
        }
        */
    }

    void Start() {
    }

    void Update() {

        #region v2
        if (useV2 == true) {

            List<AudioSource> ASToRemove = new List<AudioSource>();

            foreach (AudioSource AuSo in audioSourcesList) {
                if (AuSo != null) {
                    if (AuSo.isPlaying == false) {
                        ASToRemove.Add(AuSo);
                    }
                }
                else Debug.LogWarning("There is a null element in the audioSourcesList. That might just be that way idk");
            }

            foreach (AudioSource AuSoToRemove in ASToRemove) {
                audioSourcesList.Remove(AuSoToRemove);
                Destroy(AuSoToRemove);
            }
        }
        #endregion
    }

    public void PlaySound(string _soundName) {
        if (GameControllerScript.SoundIsMuted == false && mute == false) {
            if (useV2 == false) {
                SoundData sd = GetSoundData(_soundName);
                sd.audioSource.volume = sd.volume;
                sd.audioSource.pitch = sd.pitch;
                sd.audioSource.loop = sd.loop;
                sd.audioSource.Play();
            }
            else {
                #region v2
                AudioSource audioS = gameObject.AddComponent<AudioSource>();

                AudioClip ac = GetSound(_soundName);
                if (ac != null) {

                    audioSourcesList.Add(audioS);

                    audioS.clip = ac;
                    audioS.Play();
                }
                #endregion
            }
        }
    }

    private SoundData GetSoundData(string _soundName) {
        foreach (SoundData sD in soundData) {
            if (sD.SoundClip.name == _soundName)
                return sD;
        }
        Debug.LogError("There is no SoundData for the Sound with the Name: \"" + _soundName + "\".");
        return null;
    }

    //only used in v2
    private AudioClip GetSound(string _soundName) {
        foreach (AudioClip ac in sounds) {
            if (ac.name == _soundName)
                return ac;
        }
        Debug.LogError("There is no Sound with the Name: \"" + _soundName + "\".");
        return null;
    }
}