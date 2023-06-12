using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHel : MonoBehaviour
{
    [SerializeField] private Spawner conjureHel;
    [SerializeField] GameObject brokenBridge;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().SetCurrentRoom("Hel's Throne");
            conjureHel.enabled = true;
            brokenBridge.SetActive(true);
            StartCoroutine(WakeyWakey());
        }
    }

    IEnumerator WakeyWakey() {
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("Hel").GetComponent<Hel>().aiPath.enabled = true;
        this.GetComponent<Collider2D>().enabled = false;
        yield break;
    }
}