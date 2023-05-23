using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeDraugr : MonoBehaviour{
    // Unity Objects
    private Animator animator;
    private Rigidbody2D rigid2d;
    public AIPath aiPath;
    // Attack Variables
    private Transform meleeAttackPoint;
    private float meleeRange;
    [SerializeField] LayerMask playerLayers;
    // Stat Variables
    private int maxHealth = 150;
    private int currentHealth;


    void Start(){
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        currentHealth = maxHealth;
        animator.SetBool("isDead", false);
    }

    void Update(){
        Walk();
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
    }

    void Walk(){
        if (animator.GetFloat("xSpeed") > 0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(4);
            meleeRange = 0.5f;
        }

        else if (animator.GetFloat("xSpeed") < 0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(2);
            meleeRange = 0.5f;
        }

        else if (animator.GetFloat("ySpeed") > 0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(1);
            meleeRange = 0.55f;
        }

        else if (animator.GetFloat("ySpeed") < 0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(3);
            meleeRange = 0.8f;
        }
    }

    void Attack(){
        if (true){
            animator.SetTrigger("slash");
            // Detecting attack range
            Collider2D hitPlayer = Physics2D.OverlapCircle(meleeAttackPoint.position, meleeRange, playerLayers);
            // Deal damage

        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) return;

        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        Debug.Log("Got hit, current health is now: " + currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    void Die(){
        animator.SetBool("isDead", true);
        Debug.Log("I died");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
