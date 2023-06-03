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
    string uri = "http://localhost:5000/api/users/metrics";
    string className;
    public Stats stats;

    public void setClassName(string className) {
        this.className = className;
    }

    public IEnumerator UpdateMetrics(int kills, int wins) {
        // Create metrics object
        Metrics metrics = new Metrics();
        metrics.username = PlayerPrefs.GetString("username");
        metrics.kills = kills;
        metrics.wins = wins;

        string jsonString = JsonUtility.ToJson(metrics);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonString)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check if update was successful
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Fatal Error: Could not update metrics.");
                Debug.LogError("Reason: " + webRequest.error);
                yield break;
            }
        }
    }
}