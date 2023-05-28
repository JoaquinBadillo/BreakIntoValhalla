using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDraugr : Character {
    private Vector2 direction;
    private RangedDraugrSpriter animatorSlave;
    

    void Start() {
        maxHealth = 150;
        speed = 1.5f;
        secondaryAttack = 15;
        base.Initialize();
        animatorSlave = GetComponentInChildren<RangedDraugrSpriter>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        rigid2d.velocity = direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log("Got hit, current health is now: " + currentHealth);
        if (currentHealth <= 0)
            Die();
            
    }

    void Die() {
        animator.SetBool("isDead", true);
        Debug.Log("I died");
        // if (animatorSlave.death == true)
        //     Destroy(gameObject);
    }
}
