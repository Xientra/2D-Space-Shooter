using UnityEngine;
using System;

[Serializable]
public class SoundData {

    public AudioClip SoundClip;
    

    [Range(0,1)]
    public float volume = 1f;
    [Range(0, 3)]
    public float pitch = 1f;
    
    public bool loop = false;
    public bool isMusic = false;

    [HideInInspector]
    public AudioSource audioSource;
}