using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Hel : Character{
    public AIPath aiPath;
    [SerializeField] Transform player;
    [SerializeField] float range;
    private HelSpriter animatorSlave;

    void Start() {
        maxHealth = 2000;
        endLag = 5f;
        range = 3f;

        base.Initialize();

        animatorSlave = this.gameObject.transform.GetChild(0).GetComponent<HelSpriter>();
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        Attack();
    }

    void Attack() {
        if (Vector3.Distance(transform.position, player.transform.position) < range){
            if (Time.time >= timeUntilNextAttack){
                animator.SetTrigger("slash");
                timeUntilNextAttack = Time.time + endLag;
            }
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) 
            return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
            
    }

    void Die() {
        // Weird stuff needs to happen here...
        // Hel doesn't really die, cinematic happens
        animator.SetBool("isDead", true);
        Debug.Log("I died... SIKE!");

        Debug.Log("Yeets player out of existence");
        if (animatorSlave.death == true)
            Destroy(gameObject);
        
        
    }
}