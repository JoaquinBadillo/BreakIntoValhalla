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
    // Audio Variables
    public AudioSource audio;
    public AudioClip slash;
    public bool canAttack;

    void Start() {
        maxHealth = 60;
        endLag = 3f;
        base.Initialize();
        animatorSlave = this.gameObject.transform.GetChild(0).GetComponent<MeleeDraugrSpriter>();
        audio = GetComponent<AudioSource>();
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canAttack = true;
    }

    void Update() {
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        ICanSmellYou();
    }

    public void Attack() {
        if (Time.time >= timeUntilNextAttack && canAttack){
            aiPath.enabled = false;
            canAttack = false;
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
        StartCoroutine(Flashing());
        currentHealth -= damage;
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