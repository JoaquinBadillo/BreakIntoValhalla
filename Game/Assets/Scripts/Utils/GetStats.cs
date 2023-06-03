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


public class GetStats : MonoBehaviour {
    string uri = "http://localhost:5000/api/characters/";

    public IEnumerator FetchStats(System.Action<string> callback) {
        string endpoint = uri + PlayerPrefs.GetInt("classIndex") + "/stats";
        Debug.Log(endpoint);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
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


