/*
    Credits Scene Script
    
    Simple script to end credits scene on animation 
    end (Finish is binded to last animation frame)

    Joaquín Badillo
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
    [SerializeField] GameObject panel;
    IEnumerator Start() {
        yield return new WaitForSeconds(1f);
        for (float f = 1f; f >= 0; f -= 0.05f) {
            Color c = new Color(0, 0, 0, f);
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
    public IEnumerator Finish() {
        yield return StartCoroutine(GetComponent<MetricsManager>().UpdateMetrics(1));
        yield return StartCoroutine(GetComponent<LevelManager>().UpdateLevelData());
        SceneManager.LoadScene("LoginScreen");
    }
}
