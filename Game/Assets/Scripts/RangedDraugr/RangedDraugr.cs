using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDraugr : Character { 
    private Vector2 direction;
    // Slave Scripts
    private RangedDraugrSpriter animatorSlave;
    [SerializeField] GameObject projectile; // reference to the projectile
    public Transform player; // reference to the player
    public Vector2 facingDirection;
    public float angle; 


    void Start() {
        maxHealth = 15;
        speed = 1.5f;
        secondaryAttack = 15;
        base.Initialize();
        animatorSlave = GetComponentInChildren<RangedDraugrSpriter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        facingDirection = player.position - transform.position;
        facingDirection.Normalize();
        animator.SetFloat("facingX", facingDirection.x);
        animator.SetFloat("facingY", facingDirection.y);
    }

    public void Attack() {
        if (Time.time >= timeUntilNextShot){
            animator.SetTrigger("shoot");
            timeUntilNextShot = Time.time + secondaryEndLag;
        }
    }

    public void TakeDamage(int damage) {
        StartCoroutine(Flashing());
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die() {
        animator.SetBool("isDead", true);
    }
}
