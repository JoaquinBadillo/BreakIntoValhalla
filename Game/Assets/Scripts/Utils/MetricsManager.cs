/*
    Utility script to update player metrics

    Used by the game manager to change the player's kills
    and wins after finishing a game, either by winning or
    losing

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Metrics {
    public string username;
    public int kills;
    public int wins;
}

public class MetricsManager : MonoBehaviour {
    [SerializeField] HostSO host;

    public IEnumerator UpdateMetrics(int _wins) {
        // Create metrics object
        Metrics metrics = new Metrics();
        metrics.username = PlayerPrefs.GetString("username");
        if (PlayerPrefs.HasKey("kills"))
            metrics.kills = PlayerPrefs.GetInt("kills");
        else
            metrics.kills = 0;

        metrics.wins = _wins;

        string jsonString = JsonUtility.ToJson(metrics);
        
        string endpoint = host.uri + "users/metrics";
        using (UnityWebRequest webRequest = UnityWebRequest.Put(endpoint, jsonString)) {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            
            Debug.Log("Sending request to " + endpoint);
            yield return webRequest.SendWebRequest();

            // Check if update was successful
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Fatal Error: Could not update metrics.");
                Debug.LogError("Reason: " + webRequest.error);
            }

            else {
                Debug.Log("Metrics updated successfully!");
            }

            yield break;
        }
    }
}