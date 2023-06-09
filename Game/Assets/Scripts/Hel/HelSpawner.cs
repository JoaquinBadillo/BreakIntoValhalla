/* 
    Enemy Spawner

    Given a prefab and a list of locations, it will 
    instantiate the prefab at each location.

    Pablo Bolio with Joaquin Badillo 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelSpawner : MonoBehaviour
{
    [SerializeField] GameObject _bowDraugrPrefab;
    [SerializeField] List<GameObject> _bowDraugrLocations;
    public bool spawnable = true;

    public void Spawn() {
        foreach (GameObject location in _bowDraugrLocations) {
            Instantiate(_bowDraugrPrefab, location.transform.position, Quaternion.identity);
        }
        spawnable = false;
    }
}
