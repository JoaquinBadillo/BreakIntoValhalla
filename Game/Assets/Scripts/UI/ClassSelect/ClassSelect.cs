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
using UnityEngine.Networking;


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
		StartCoroutine(GetComponent<CharacterManager>().SelectCharacter());
	}
}
