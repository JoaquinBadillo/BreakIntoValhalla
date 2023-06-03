using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameThingy {
	public string username;
	public int character_id;
}
public class ClassSelect : MonoBehaviour {
    public GameObject[] classArray;
	public int classIndex = 0;

	public void Next() {
		classArray[classIndex].SetActive(false);
		classIndex = (classIndex + 1) % classArray.Length;
		classArray[classIndex].SetActive(true);
	}

	public void Previous() {
		classArray[classIndex].SetActive(false);
		classIndex--;
		if (classIndex < 0)
		{
			classIndex += classArray.Length;
		}
		classArray[classIndex].SetActive(true);
	}

	public void StartGame() {
		Debug.Log("Ayo stop f poking me!");
		// API stuff
		PlayerPrefs.SetInt("classIndex", classIndex);
		StartCoroutine(CreateGame());
		//SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public IEnumerator CreateGame() {
		string uri = "http://localhost:5000/api/game";

		// Create User object
        GameThingy game = new GameThingy();
        game.username = PlayerPrefs.GetString("username");
        game.character_id = PlayerPrefs.GetInt("classIndex");

        string jsonString = JsonUtility.ToJson(game);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
			yield return webRequest.SendWebRequest();

			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + webRequest.error);
                yield break;
            }
		}

		string endpoint = uri + "/" + PlayerPrefs.GetString("username") + "/levels";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                jsonString = webRequest.downloadHandler.text;
                Level level = JsonUtility.FromJson<Level>(webRequest.downloadHandler.text);
				Debug.Log(level.seed);
                PlayerPrefs.SetInt("seed", level.seed);
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
