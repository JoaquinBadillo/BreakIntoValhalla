/*
    Utility script to fetch stats from the database

    Add as component to a game object and call FetchStats
    with a callback that uses the json string to set the
    stats of the player

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Stats {
    // HP
    public int hp;
    // Attack Damage for each weapon
    public int primary_attack;
    public int secondary_attack;
    // Attack Speed for each weapon
    public float primary_lag;
    public float secondary_lag;
    // Defense
    public int defense;
    // Movement Speed
    public float speed;
}

public class GetStats : MonoBehaviour {
    string uri = "http://localhost:5000/api/classes/";
    string classType;
    public Stats stats;

    public void setClassType(string classType) {
        this.classType = classType;
    }

    public IEnumerator FetchStats(System.Action<string> callback) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + classType + "/stats")) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check if fetching was successful and send json to callback
            if (webRequest.result == UnityWebRequest.Result.Success) {
                string jsonString = webRequest.downloadHandler.text;
                callback(jsonString);   
            }

            else
                Debug.Log("Error: " + webRequest.error);
        }
    }
}


