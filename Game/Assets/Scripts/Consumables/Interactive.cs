/*
    Interactive Abstract Class

    Defines common behaviours for interactive objects.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour {
    protected bool interactable;
    protected bool finished = false;

    // Interactive objects save the player that interacted with them
    protected Player player;

    // The Interact method is what makes each interactive object unique!
    virtual public void Interact() {}

    // PLayers interact with objects if they are in range (inside trig.) 
    // and press F
    protected void OnTriggerStay2D(Collider2D other) {
        if(finished) Destroy(this.gameObject);
        if (!interactable) return;
        if (other.gameObject.tag != "Player") return;
        
        // TODO: Show interact key when nearby

        if (Input.GetKeyDown(KeyCode.F)) {
            player = other.gameObject.GetComponent<Player>();
            // Avoid multiple interaction shenanigans
            interactable = false;
            finished = false;
            Interact();
        }
    }
}
