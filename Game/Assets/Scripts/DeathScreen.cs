using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class LevelData {
    public string username;
    public int level_num;
}

public class LevelResponse {
    public int seed;
}

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start() {
        string uri = "http://localhost:5000/api/users/levels";

        // Create Level object
        LevelData data = new LevelData();
        data.username = PlayerPrefs.GetString("username");
        data.level_num = 1;

        string jsonString = JsonUtility.ToJson(data);

        Debug.Log(jsonString);

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonString)) {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log("Error: " + webRequest.error);
                PlayerPrefs.SetInt("seed", Random.Range(1, 800000));
            } else {
                Debug.Log("Success");
                jsonString = webRequest.downloadHandler.text;
                Debug.Log(jsonString);
                LevelResponse level = JsonUtility.FromJson<LevelResponse>(webRequest.downloadHandler.text);
			    Debug.Log(level.seed);
                PlayerPrefs.SetInt("seed", level.seed);
            }
            
            

            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("MainScene");
            yield break;
        }
    }

}
