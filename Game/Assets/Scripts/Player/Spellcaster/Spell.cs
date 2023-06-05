using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    private int attack;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Saber Draugr")){
            other.GetComponent<MeleeDraugr>().TakeDamage(attack);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Bow Draugr")){
            other.GetComponent<RangedDraugr>().TakeDamage(attack);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Hel")){
            other.GetComponent<Hel>().TakeDamage(attack);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Walls"))
            Destroy(gameObject);
    }

    public void SetAttack(int damage){
        attack = damage;
    }
}