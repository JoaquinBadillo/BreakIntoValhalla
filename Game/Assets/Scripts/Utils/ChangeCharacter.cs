using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeCharacter : Interactive {
    private void Start() {
        interactable = true;
    }

    override public void Interact() {
        SceneManager.LoadScene("ClassSwitch");
    }
}
