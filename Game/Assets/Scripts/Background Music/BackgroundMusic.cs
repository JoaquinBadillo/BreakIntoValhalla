using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    public static BackgroundMusic instance = null;
    public static BackgroundMusic Instance { get { return instance; } }

    public AudioSource audio;
    public AudioClip berserkir;
    public AudioClip FlokisLastJourney;
    public AudioClip Hel;

    void Start() {
        audio = GetComponent<AudioSource>();
        audio.clip = berserkir;
        audio.Play();
    }
    
    void Awake() {
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        else
            instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
}
