/* 
    Enemy Spawner

    Given a prefab and a list of locations, it will 
    instantiate the prefab at each location.

    Joaquin Badillo 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] List<GameObject> _locations;
    void Start() {
        foreach (GameObject location in _locations) {
            Instantiate(_prefab, location.transform.position, Quaternion.identity);
        }
    }
}
