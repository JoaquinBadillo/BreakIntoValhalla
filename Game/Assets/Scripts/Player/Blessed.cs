using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessed : MonoBehaviour {
    private SpriteRenderer spriter;
    private float angle;
    private float opacity;
    void Start(){
        spriter = GetComponent<SpriteRenderer>();
        spriter.enabled = false;
        angle = 0;
    }
    void Update() {
        opacity = 0.2f * Mathf.Sin(angle) + 0.8f;
        if (spriter.enabled == true)
            spriter.color = new Color(1f,1f,1f, opacity);
        angle += 0.05f; 
    }
    public void AuraOn(){
        spriter.enabled = true;
    }
    public void AuraOff(){
        spriter.enabled = false;
    }
}
