/*
    Utility script to manage reading and updating levels

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Level {
    public int level_num;
    public int seed;
}

public class LevelResponse {
    public int seed;
}

public class LevelData {
    public string username;
    public int level_num;
}

public class LevelManager : MonoBehaviour {
    [SerializeField] HostSO host;

    public IEnumerator ReadLevelData() {
        // Load Level Data
		string endpoint = host.uri + "levels/" + PlayerPrefs.GetString("username");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                Debug.Log(webRequest.downloadHandler.text);
                Level level = JsonUtility.FromJson<Level>(webRequest.downloadHandler.text);
				Debug.Log(level.seed);
                PlayerPrefs.SetInt("seed", level.seed);
            }

			else {
				Debug.Log("Error: " + webRequest.error);
				PlayerPrefs.SetInt("seed", Random.Range(0, 80000));
			}
        }
    }

    public IEnumerator UpdateLevelData() {
        // Create Level object
        LevelData data = new LevelData();
        data.username = PlayerPrefs.GetString("username");
        data.level_num = 1;

        string jsonString = JsonUtility.ToJson(data);

        string endpoint = host.uri + "levels";

        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log("Error: " + webRequest.error);
                PlayerPrefs.SetInt("seed", Random.Range(1, 800000));
            } 
            
            else {
                jsonString = webRequest.downloadHandler.text;
                LevelResponse level = JsonUtility.FromJson<LevelResponse>(webRequest.downloadHandler.text);
			    Debug.Log(level.seed);
                PlayerPrefs.SetInt("seed", level.seed);
            }
        }
    }
}