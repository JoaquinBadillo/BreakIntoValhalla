/*
    Chest

    Allows the player to buff their stats when
    they interact with the "upgrade" object.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    private bool interactable = true;

    // Items that the chest could drop
    [SerializeField] List<GameObject> drops;
    
    // Probabilities for each item expressed as percentages, must add to 100
    [SerializeField] List<int> dropRates;

    // The item a particular chest will drop
    private GameObject storedItem;

    private void Start() {
        int value = Random.Range(0, 100);

        int acc = 0;
        for (int i = 0; i < dropRates.Count; i++) {
            if (value < acc + dropRates[i]) {
                storedItem = drops[i];
                break;
            }
        }

    }

    private void Interact() {
        // TODO: Show interact key when nearby

        if (Input.GetKeyDown(KeyCode.F)) {
            // Avoid multiple interaction shenanigans
            interactable = false;

            Instantiate(storedItem, this.transform.position, Quaternion.identity);
            
            // Could potentially add coroutine to wait for animation/SFX to end
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!interactable) return;
        if (other.gameObject.tag != "Player") return;
        
        Interact();
    }
}
