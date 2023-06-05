/*
    Potion

    Allows the player to heal according to the potion's
    healing rate when they interact with the "potion" object.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Interactive {

    [SerializeField] float healingRate;
    private void Start() {
        interactable = true;
    }

    override protected void Interact() {
        player.Heal(healingRate);
        Destroy(this.gameObject);
    }
}
