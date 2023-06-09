/*
    Death Scene Script

    Has a coroutine start method that sends a request to the server
    to update the user's level seed and send them back to level 1 (currently the only one). 
    If the request fails, the seed is set locally to a random number.

    The endpoint sends the new level data back to the client, which is then used to set the seed
    in Unity.

    It also waits 2 seconds to change between scenes so that the death screen doesn't flash.

    Joaquín Badillo
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class DeathData {
    public string username;
    public string killer;
    public string room;
}

public class DeathScreen : MonoBehaviour {
    [SerializeField] HostSO host;

    IEnumerator Start() {
        yield return StartCoroutine(GetComponent<MetricsManager>().UpdateMetrics(0));
        yield return StartCoroutine(GetComponent<LevelManager>().UpdateLevelData());
            
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainScene");
        yield break;
    }

    IEnumerator SendDeathData() {
        DeathData data = new DeathData();
        data.username = PlayerPrefs.GetString("username");
        data.killer = PlayerPrefs.GetString("killer");
        data.room = PlayerPrefs.GetString("room");

        if (data.killer == "" || data.room == "" ) {
            Debug.LogError("Failed to load data");
            yield break;
        }

        
        string jsonString = JsonUtility.ToJson(data);
        string endpoint = host.uri + "deaths";
        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error sending death data!");
                Debug.LogError("Error: " + webRequest.error);
            }

            else {
                Debug.Log("Death data sent successfully!");
            }
                
            yield break;
        }
    }

}
