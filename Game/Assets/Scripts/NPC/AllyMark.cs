/*
    Ally Marks
    Simple script to make ally marks (green triangles) move up and down

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMark : MonoBehaviour {
    [SerializeField] float amplitude;

    private void Update() {
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(2 * Time.time) * amplitude);
    }
}