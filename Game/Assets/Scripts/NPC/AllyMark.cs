using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMark : MonoBehaviour {
    [SerializeField] float amplitude;

    private void Update() {
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(2 * Time.time) * amplitude);
    }
}