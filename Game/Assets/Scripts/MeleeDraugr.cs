using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeDraugr : Character{
    // Astar Object
    public AIPath aiPath;
    // Attack Variables
    // reach
    [SerializeField] Transform player;
    [SerializeField] float range;
    // Slave script for animator events
    private MeleeDraugrSpriter animatorSlave;


    void Start() {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        animatorSlave = this.gameObject.transform.GetChild(0).GetComponent<MeleeDraugrSpriter>();
        currentHealth = maxHealth;
        animator.SetBool("isDead", false);
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        Walk();
        Attack();
    }

    void Walk() {
        if (animator.GetFloat("xSpeed") > 0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(2);
            meleeRange = 1f;
        }

        else if (animator.GetFloat("xSpeed") < -0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(1);
            meleeRange = 1f;
        }
    }

    void Attack() {
        if (Vector3.Distance(transform.position, player.transform.position) < range){
            animator.SetTrigger("slash");
            timeUntilNextAttack = Time.time + endLag;
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) 
            return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
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
        if (animatorSlave.endOfAnimation() == true)
            this.gameObject.SetActive(false);
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
            GetComponent<AIPath>().enabled = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
            GetComponent<AIPath>().enabled = false;
    }
}