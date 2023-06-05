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

    override protected void Interact() {
        player.SetKeyStatus(true);
        Destroy(this.gameObject);
    }
}
