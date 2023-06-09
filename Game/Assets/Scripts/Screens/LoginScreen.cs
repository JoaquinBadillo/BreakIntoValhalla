/*
    Login Screen Script

    Allows the user to tab between fields and login with the enter key.

    It reads the username, email, and password fields and sends them to the server
    to either login or register the user. If the request fails, it displays an error message.

    It handles connection errors and protocol errors (most likely username or email already exists)
    separately to display more descriptive error messages.

    User is sent to their game scene or to character select according to the model's logic:
    a game id is required to play the game.

    Joaquín Badillo, Pablo Bolio
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class User {
    public string username;
    public string email;
    public string password;
    public int game_id;
}

public class LoginScreen : MonoBehaviour {
    EventSystem system;
    public Selectable first;
    [SerializeField] TMP_Text username;
    [SerializeField] TMP_Text email;
    [SerializeField] TMP_Text password;
    [SerializeField] TMP_Text errorMessage;

    string uri = "https://valhallaapi-production.up.railway.app/api";

    public Button submit;
    // Start is called before the first frame update
    void Start() {
        system = EventSystem.current;
        first.Select();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
                next.Select();
            else
                first.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Return)){
            submit.onClick.Invoke();
            Debug.Log("Gotta love the return key");
        }
    }

    public void Login() {
        errorMessage.text = "";

        if (username.text == "" || email.text == "" || password.text == "") {
            errorMessage.text = "Please fill out all fields";
            return;
        }
        
        StartCoroutine(WebLogin(username.text, email.text, password.text));
        

        Debug.Log("Yeet");
               
    }

    public void Register() {
        errorMessage.text = "";

        if (username.text == "" || email.text == "" || password.text == "") {
            errorMessage.text = "Please fill out all fields";
            return;
        }
        
        StartCoroutine(WebRegister(username.text, email.text, password.text));
    }

     public IEnumerator WebRegister(string username, string email, string password) {
        // Create User object
        User newUser = new User();
        newUser.username = username;
        newUser.email = email;
        newUser.password = password;

        string jsonString = JsonUtility.ToJson(newUser);

        Debug.Log(jsonString);
        
        string endpoint = uri + "/users";
        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for the desired page.
            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            // Check for server connection errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
                errorMessage.text = "Server unreachable";
                Debug.LogError("Error: " + webRequest.error);
                yield break;
            }
            // Check for request errors (most likely username or email already exists)
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError) {
                errorMessage.text = "Try with another username or email";
                Debug.LogError("Error: " + webRequest.error);
                yield break;
            }

            // If no errors, save user data to PlayerPrefs file and go to class select
            PlayerPrefs.SetString("username", newUser.username);
            PlayerPrefs.SetString("email", newUser.email);
            PlayerPrefs.SetString("password", newUser.password);

            SceneManager.LoadScene("ClassSelect");
            yield break;
        }
    }


    public IEnumerator WebLogin(string username, string email, string password) {
        // Create User object
        User newUser = new User();
        newUser.username = username;
        newUser.email = email;
        newUser.password = password;

        string jsonString = JsonUtility.ToJson(newUser);
        
        string endpoint = uri + "/users/login";
        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            Debug.Log("Sending request to " + endpoint);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check for server connection errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
                errorMessage.text = "Server unreachable";
                Debug.LogError("Error: " + webRequest.error);
                yield break;
            }
            // Check for request errors (most likely username or email already exists)
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError) {
                errorMessage.text = "Invalid credentials";
                Debug.LogError("Error: " + webRequest.error);
                yield break;
            }

            User user = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);

            
            PlayerPrefs.SetString("username", user.username);
            PlayerPrefs.SetString("email", user.email);
            PlayerPrefs.SetString("password", user.password);

            if (user.game_id == 0) {
                SceneManager.LoadScene("ClassSelect");
                yield break;
            }
        }

        endpoint = uri + "/levels/" + PlayerPrefs.GetString("username");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                jsonString = webRequest.downloadHandler.text;
                Level level = JsonUtility.FromJson<Level>(webRequest.downloadHandler.text);
                PlayerPrefs.SetInt("seed", level.seed);
                Debug.Log("Seed: " + level.seed);
            }

            else {
				Debug.Log("Error: " + webRequest.error);
				yield break;
			}

            SceneManager.LoadScene("MainScene");
            yield break;
        }
        
    }



}
