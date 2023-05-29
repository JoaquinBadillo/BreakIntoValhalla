using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDraugrAttacker : MonoBehaviour {
    void OnTriggerStay2D(Collider2D other) {
        this.GetComponentInParent<MeleeDraugr>().Attack();
    }
}
