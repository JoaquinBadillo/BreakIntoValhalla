/*
    Script to set the player's current room
    (Used to save the place where the player died)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] string roomName;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().SetCurrentRoom(roomName);
        }
    }
}
