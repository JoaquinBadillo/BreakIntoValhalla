using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeDraugrChaser : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
            this.GetComponentInParent<AIPath>().enabled = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
            this.GetComponentInParent<AIPath>().enabled = false;
    }
}
