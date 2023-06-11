using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerAfterimage : Afterimage {
    void Start() {
        base.Initialize();
    }

    void Update() {
        base.Refurbish();
    }


    override public void Right() {
        spriter1 = master.gameObject.transform.GetChild(23).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(24).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(25).GetComponent<SpriteRenderer>();
    }
    override public void Left() {
        spriter1 = master.gameObject.transform.GetChild(17).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(18).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(19).GetComponent<SpriteRenderer>();
    }
    override public void Up() {
        spriter1 = master.gameObject.transform.GetChild(14).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(15).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(16).GetComponent<SpriteRenderer>();
    }
    override public void Down() {
        spriter1 = master.gameObject.transform.GetChild(20).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(21).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(22).GetComponent<SpriteRenderer>();
    }
    override public void LeftUp(){
        spriter1 = master.gameObject.transform.GetChild(26).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(27).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(28).GetComponent<SpriteRenderer>();
    }
    override public void RightUp(){
        spriter1 = master.gameObject.transform.GetChild(29).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(30).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(31).GetComponent<SpriteRenderer>();
    }
    override public void LeftDown(){
        spriter1 = master.gameObject.transform.GetChild(32).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(33).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(34).GetComponent<SpriteRenderer>();
    }
    override public void RightDown(){
        spriter1 = master.gameObject.transform.GetChild(35).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(36).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(37).GetComponent<SpriteRenderer>();
    }
    
}
