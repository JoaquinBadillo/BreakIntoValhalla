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
    public HostSO host;
    public Button submit;
    private BackgroundMusic musicMaster;

    // Start is called before the first frame update
    void Start() {
        system = EventSystem.current;
        first.Select();
        musicMaster = GameObject.FindGameObjectWithTag("Sounder").GetComponent<BackgroundMusic>();
        StartCoroutine(MusicFadeIn());
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

    IEnumerator MusicFadeIn(){
        for (float f = musicMaster.audio.volume; f >= 0; f += 0.05f) {
            musicMaster.audio.volume = f;
            yield return new WaitForSeconds(0.05f);
            if (!musicMaster.audio.isPlaying)
                musicMaster.audio.Play();
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
        
        string endpoint = host.uri + "users";
        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

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
        
        string endpoint = host.uri + "users/login";
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

            // If game does not exist (id == 0), then go to class select and stop coroutine
            if (user.game_id == 0) {
                SceneManager.LoadScene("ClassSelect");
                yield break;
            }
        }

        // Load character data
        yield return StartCoroutine(GetComponent<CharacterManager>().LoadCharacterData());

        // Load level data
        yield return StartCoroutine(GetComponent<LevelManager>().ReadLevelData());

        SceneManager.LoadScene("MainScene");
        yield break;  
    }
}
