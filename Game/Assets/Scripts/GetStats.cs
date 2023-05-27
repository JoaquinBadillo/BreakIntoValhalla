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
    // Stat id
    public int stats_id;
    // Class id
    public int class_id;
    // HP
    public int hp;
    // ATK
    public int attack;
    // ATKSPD
    public float attack_speed;
    // DEF
    public int defense;
    // SPD
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


