/*
    Interactive Abstract Class

    Defines common behaviours for interactive objects.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    protected bool interactable;

    protected Player player;

    virtual protected void Interact() {}

    protected void OnTriggerStay2D(Collider2D other) {
        if (!interactable) return;
        if (other.gameObject.tag != "Player") return;
        
        // TODO: Show interact key when nearby

        if (Input.GetKeyDown(KeyCode.F)) {
            player = other.gameObject.GetComponent<Player>();
            // Avoid multiple interaction shenanigans
            interactable = false;
            Interact();
        }
    }
}
