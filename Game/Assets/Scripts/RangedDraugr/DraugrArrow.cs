using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrArrow : MonoBehaviour {
    private int attack;
    private Rigidbody2D rigid2d;
    private Collider2D myPointySelf;
    public Collider2D EnemyCollider { get; set;}
    private float returnSpeed;
    private Vector2 direction; 

    void Start() {
        myPointySelf = GetComponent<Collider2D>();
        IgnoreCollisionsCheck();
        rigid2d = GetComponent<Rigidbody2D>();
        returnSpeed = 20f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            Debug.Log(other.name);
            other.GetComponent<Player>().TakeDamage(attack);
            Debug.Log("Damage dealt: " + attack);
            YeetArrow();
        }
        else if (other.CompareTag("Walls"))
            YeetArrow();
    }

    public void YeetArrow() {
        Destroy(gameObject);
    }    

    public void SetAttack(int damage) {
        attack = damage;
    }

    public void IgnoreCollisionsCheck() {
        if(!Physics2D.GetIgnoreCollision(myPointySelf, EnemyCollider))
            Physics2D.IgnoreCollision(myPointySelf, EnemyCollider, true);
        else
            Physics2D.IgnoreCollision(myPointySelf, EnemyCollider, false);       
    }

    public void Deflect(Vector2 newDirection) {
        IgnoreCollisionsCheck();
        rigid2d.velocity = newDirection * returnSpeed;
    }
}