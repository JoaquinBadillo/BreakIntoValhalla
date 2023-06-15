using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelMusic : MonoBehaviour {
    public BackgroundMusic musicMaster;
    // Start is called before the first frame update
    void Start() {
        musicMaster = GameObject.FindGameObjectWithTag("Sounder").GetComponent<BackgroundMusic>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            musicMaster.audio.Stop();
            musicMaster.audio.clip = musicMaster.Hel;
            musicMaster.audio.Play();
        }
    }
}
