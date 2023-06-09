/*
    Chest

    Allows the player to buff their stats when
    they interact with the "upgrade" object.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactive {
    // Items that the chest could drop
    [SerializeField] List<GameObject> drops;
    
    // Probabilities for each item expressed as percentages, must add to 100
    [SerializeField] List<int> dropRates;

    // The item a particular chest will drop
    private GameObject storedItem;

    private void Start() {
        interactable = true;
        int value = Random.Range(0, 100);

        int acc = 0;

        // Determine the item to drop
        for (int i = 0; i < drops.Count; i++) {
            // Probability distribution thingy
            if (value < acc + dropRates[i]) {
                storedItem = drops[i];
                break;
            }
            acc += dropRates[i];
        }

    }

    override public void Interact() {
        Instantiate(storedItem, this.transform.position, Quaternion.identity);
        finished = true;
        // Could potentially add coroutine to wait for animation/SFX to end
    }
}
