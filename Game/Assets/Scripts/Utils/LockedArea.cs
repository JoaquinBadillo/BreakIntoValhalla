/*
    Locked Area Script

    Checks if the player has the key to unlock the final boss area.
    In the level currently designed the key is a hammer, this allows
    the player to repair a broken bridge and reach the final boss.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedArea : MonoBehaviour {
    [SerializeField] GameObject blocking;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (other.gameObject.GetComponent<Player>().HasKey()) {
                blocking.SetActive(false);
            }
        }
    }
}
