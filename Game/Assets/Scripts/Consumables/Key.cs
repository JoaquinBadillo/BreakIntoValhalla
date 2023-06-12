/*
    Key Script

    Allows the player to reach the final boss!
    Players have a key status, which is set to true when they interact with
    the key.

    Joaquin Badillo
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactive {
    void Start() {
        interactable = true;
    }

    override public void Interact() {
        player.SetKeyStatus(true);
        GameObject.FindGameObjectWithTag("NPC").GetComponent<Dialogue>().SetText("The greatest trial of all\nLies ahead");
        finished = true;
    }
}
