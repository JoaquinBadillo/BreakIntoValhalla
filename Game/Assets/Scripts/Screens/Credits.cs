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

public class Credits : MonoBehaviour
{
    public IEnumerator Finish() {
        yield return StartCoroutine(GetComponent<MetricsManager>().UpdateMetrics(1));
        yield return StartCoroutine(GetComponent<LevelManager>().UpdateLevelData());
        SceneManager.LoadScene("LoginScreen");
    }
}
