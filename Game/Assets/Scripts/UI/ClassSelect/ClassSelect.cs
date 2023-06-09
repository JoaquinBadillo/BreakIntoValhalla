/* 
    Character Select Screen

    This script has mainly 2 purposes:
	1. To allow the user to select a character from an array of prefabs
	2. To send the user's character selection to the server
		2.1. Character selection creates a new game in the database, with a random seed
			 Consult the ER diagram if you want to know more about the database structure

    Joaquin Badillo, Pablo Bolio
*/

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
		PlayerPrefs.SetInt("classIndex", classIndex + 1);
		Debug.Log(PlayerPrefs.GetInt("classIndex"));
		StartCoroutine(CreateGame());
	}

	public IEnumerator CreateGame() {
		//string uri = "https://valhallaapi-production.up.railway.app/api";
		string uri = "http://localhost:5000/api/";
		// Create Game object
        GameThingy game = new GameThingy();
        game.username = PlayerPrefs.GetString("username");
        game.character_id = PlayerPrefs.GetInt("classIndex");

        string jsonString = JsonUtility.ToJson(game);
		
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri + "game", jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
			yield return webRequest.SendWebRequest();

			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error creating game!");
				Debug.LogError("Error: " + webRequest.error);
                yield break;
            }
		}

		string endpoint = uri + "levels/" + PlayerPrefs.GetString("username");
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
