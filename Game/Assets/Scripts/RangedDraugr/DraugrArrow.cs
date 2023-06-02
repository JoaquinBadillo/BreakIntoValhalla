using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrArrow : MonoBehaviour {
    private int attack;
    private Collider2D myPointySelf;
    public Collider2D enemyCollider { get; set;}

    void Start() {
        myPointySelf = GetComponent<Collider2D>();
        //IgnoreCollisionsCheck();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log(other.name);
            other.GetComponent<Player>().TakeDamage(attack);
            Debug.Log("Damage dealt: " + attack);
            YeetArrow();
        }
        else if (other.CompareTag("Walls"))
            YeetArrow();
    }

    public void YeetArrow(){
        Destroy(gameObject);
    }    

    public void SetAttack(int damage){
        attack = damage;
    }

    // public void IgnoreCollisionsCheck() {
    //     if(!physics2D.GetIgnoreCollision(myPointySelf, enemyCollider))
    //         physics2D.IgnoreCollision(myPointySelf, enemyCollider, true);
    //     else
    //         physics2D.IgnoreCollision(myPointySelf, enemyCollider, false);
            
    // }
}