/*
    Utility script to manage character selection (game creation) and loading

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CharacterData {
    public int character_id;
}

public class GameThingy {
	public string username;
	public int character_id;
}

public class CharacterManager : MonoBehaviour {
    public HostSO host;

    public IEnumerator LoadCharacterData() {
        // Load character data
        string endpoint = host.uri + "characters/" + PlayerPrefs.GetString("username");

        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                string jsonString = webRequest.downloadHandler.text;
                CharacterData data = JsonUtility.FromJson<CharacterData>(webRequest.downloadHandler.text);
                PlayerPrefs.SetInt("classIndex", data.character_id);
                Debug.Log("Class Index: " + data.character_id);
            }

            else {
				Debug.Log("Error: " + webRequest.error);
				PlayerPrefs.SetInt("classIndex", 4);
			}
        }
    }

    // Create a game with the selected character
    public IEnumerator SelectCharacter() {
		// Create Game object
        GameThingy game = new GameThingy();
        game.username = PlayerPrefs.GetString("username");
        game.character_id = PlayerPrefs.GetInt("classIndex");

        string jsonString = JsonUtility.ToJson(game);
		
        string endpoint = host.uri + "game";
        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
			yield return webRequest.SendWebRequest();

			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error creating game!");
				Debug.LogError("Error: " + webRequest.error);
				PlayerPrefs.SetInt("seed", Random.Range(0, 80000));
            }
		}

        SceneManager.LoadScene("MainScene");
        yield break;
	}

}