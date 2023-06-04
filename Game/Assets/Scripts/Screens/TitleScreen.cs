/* 
    Title Screen

    This script is used to control the title screen.
    
    It has two functions: PlayGame and QuitGame; these
    are called by the buttons in the title screen.

    Joaquin Badillo 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {
    // Note: When building the game, make sure to set the build index of play scene to 1
    public void PlayGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScreen");
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }


}
