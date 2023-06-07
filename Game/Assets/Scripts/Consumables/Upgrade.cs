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
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpgradeHealth();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        finished = true;
    }

    public void UpgradeAttack() {
        Debug.Log("Upgraded Attack");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpgradeAttack();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        finished = true;
    }

    public void UpgradeSpeed() {
        Debug.Log("Upgraded Speed");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpgradeSpeed();
        canvas.SetActive(false);
        Time.timeScale = 1f;
        finished = true;
    }

    override public void Interact() {
        canvas.SetActive(true);
        Time.timeScale = 0f;
    }
}
