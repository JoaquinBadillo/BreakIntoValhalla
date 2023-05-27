/*
    Upgrade

    Allows the player to buff their stats when
    they interact with the "upgrade" object.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {
    private bool interactable = true;
    
    [SerializeField] GameObject canvas;

    public void UpgradeHealth() {
        Debug.Log("Upgraded Health");
        // GameObject.Find("Player").GetComponent<PlayerMovement>().UpgradeHealth();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    public void UpgradeAttack() {
        Debug.Log("Upgraded Attack");
        // GameObject.Find("Player").GetComponent<PlayerMovement>().UpgradeAttack();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    public void UpgradeSpeed() {
        Debug.Log("Upgraded Speed");
        // GameObject.Find("Player").GetComponent<PlayerMovement>().UpgradeSpeed();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    private void Interact() {
        // TODO: Show interact key when nearby

        if (Input.GetKeyDown(KeyCode.F)) {
            // Avoid multiple interaction shenanigans
            interactable = false;

            canvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!interactable) return;
        if (other.gameObject.tag != "Player") return;
        
        Interact();
    }
}
