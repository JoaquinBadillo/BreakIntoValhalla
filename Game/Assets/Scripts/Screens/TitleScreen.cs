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
    public void PlayGame() {
        SceneManager.LoadScene("LoginScreen");
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }


}
