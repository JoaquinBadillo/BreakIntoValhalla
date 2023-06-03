using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadClass : MonoBehaviour
{
	public GameObject[] classPrefabs;
	public Transform spawnPoint;

	void Start()
	{
		int classIndex = PlayerPrefs.GetInt("classIndex");
		GameObject prefab = classPrefabs[classIndex];
		GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
	}
}