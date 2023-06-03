using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		// API stuff
		PlayerPrefs.SetInt("classIndex", classIndex);
		//SceneManager.LoadScene(1, LoadSceneMode.Single);
	}
}
