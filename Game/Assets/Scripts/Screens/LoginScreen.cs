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
    
    string uri = "http://localhost:5000/api/users";

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
        
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check if register process was successful
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                errorMessage.text = "Try with another username or email";
                Debug.LogError("Error: " + webRequest.error);
                yield break;
            }

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

        Debug.Log(jsonString);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri + "/login", jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check if login was successful
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                errorMessage.text = "Try with another username or email";
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

        string endpoint = uri + "/" + PlayerPrefs.GetString("username") + "/levels";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            Debug.Log("Hop in loser!");
            yield return webRequest.SendWebRequest();
            Debug.Log("We goin' insane!");

            if (webRequest.result == UnityWebRequest.Result.Success) {
                Debug.Log("Mission Accomplished");
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
