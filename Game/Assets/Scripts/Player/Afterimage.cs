using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour {
    [SerializeField] private Player master;
    [SerializeField] private float activeTime;
    [SerializeField] private float spawnRate;
    [SerializeField] private float timeUntilNextSpawn;
    [SerializeField] private GameObject afterimage;
    // Start is called before the first frame update
    void Start() {
        activeTime = 3f;
        spawnRate = 0.1f;
        master = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update() {
        if (master.isDashing)
            SpawnAfterimage();
    }

    void SpawnAfterimage() {
        if (Time.deltaTime >= timeUntilNextSpawn) {
            GameObject afterImage = Instantiate(afterimage, transform.position, Quaternion.identity);
            Destroy(afterImage, activeTime);
            timeUntilNextSpawn = Time.deltaTime + spawnRate;
        }
    }
}
