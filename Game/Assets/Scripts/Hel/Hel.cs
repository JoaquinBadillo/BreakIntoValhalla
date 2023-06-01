using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Hel : Character{
    public AIPath aiPath;
    [SerializeField] Transform player;
    [SerializeField] float range;
    private HelSpriter animatorSlave;

    public float meleeInnerRange;
    public float xRange;
    public float yRange;
    public float zRange;

    public Transform meleeAttackBox;
    public Transform meleeAttackCircle;

    public bool isAttacking;

    void Start() {
        maxHealth = 600;
        endLag = 5f;
        range = 3f;

        zRange = 1;

        base.Initialize();

        animatorSlave = this.gameObject.transform.GetChild(0).GetComponent<HelSpriter>();
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        if (Vector3.Distance(transform.position, player.transform.position) < range)
            Attack();
    }

    public void Attack() {
        if (Time.time >= timeUntilNextAttack){
            aiPath.enabled = false;
            animator.SetTrigger("slash");
            timeUntilNextAttack = Time.time + endLag;
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null || meleeAttackBox == null || meleeAttackCircle == null) 
            return;
        
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);

        if (meleeAttackBox != null)
            Gizmos.DrawWireCube(meleeAttackBox.position, new Vector3(xRange, yRange, zRange));
        
        if (meleeAttackCircle != null)
            Gizmos.DrawWireSphere(meleeAttackCircle.position, meleeRange);

    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
            
    }

    void Die() {
        // Weird stuff needs to happen here...
        // Hel doesn't really die, cinematic happens
        Debug.Log("I died...");
        animator.SetBool("isDead", true);
        Debug.Log("SIKE!");

        Debug.Log("Yeets player out of existence");
        if (animatorSlave.death == true)
            Destroy(gameObject);
         
    }
}