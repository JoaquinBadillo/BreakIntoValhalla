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
    // Slave script for animator events
    private MeleeDraugrSpriter animatorSlave;
    // distance between enemy and player
    private float reach = 8;

    void Start() {
        maxHealth = 60;
        endLag = 3f;
        base.Initialize();
        animatorSlave = this.gameObject.transform.GetChild(0).GetComponent<MeleeDraugrSpriter>();
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        ICanSmellYou();
    }

    public void Attack() {
        if (Time.time >= timeUntilNextAttack){
            aiPath.enabled = false;
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
    }
    /*
        This function allows melee enemy to start chasing the player
    */
    void ICanSmellYou(){
        if (Vector2.Distance(transform.position, player.position) <= reach)
            aiPath.enabled = true;
        else
            aiPath.enabled = false;
    }
}