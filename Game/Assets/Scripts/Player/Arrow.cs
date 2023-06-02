using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private int attack;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            Debug.Log(other.name);
            other.GetComponent<MeleeDraugr>().TakeDamage(attack);
            Debug.Log("Damage dealt: " + attack);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Walls"))
            Destroy(gameObject);
    }

    public void SetAttack(int damage){
        attack = damage;
    }
}