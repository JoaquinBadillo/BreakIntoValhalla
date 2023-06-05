/*
    Upgrade

    Allows the player to buff their stats when
    they interact with the "upgrade" object.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : Interactive {    
    [SerializeField] GameObject canvas;

    private void Start() {
        interactable = true;
    }

    public void UpgradeHealth() {
        Debug.Log("Upgraded Health");
        player.UpgradeHealth();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        finished = true;
    }

    public void UpgradeAttack() {
        Debug.Log("Upgraded Attack");
        player.UpgradeAttack();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        finished = true;
    }

    public void UpgradeSpeed() {
        Debug.Log("Upgraded Speed");
        player.UpgradeSpeed();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        finished = true;
    }

    override public void Interact() {
        canvas.SetActive(true);
        Time.timeScale = 0f;
    }
}
