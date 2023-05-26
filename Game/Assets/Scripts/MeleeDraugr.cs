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
    // hitboxRange
    private Transform meleeAttackPoint;
    [SerializeField] float hitboxRange;
    [SerializeField] LayerMask playerLayers;
    // reach
    [SerializeField] Transform player;
    [SerializeField] float range;
    // Stat Variables
    // HP
    private int maxHealth = 150;
    private int currentHealth;
    // ATK
    public int attack = 20;
    // ATKSPD
    public float attackRate = 2; // how many attacks per second
    public float nextAttackTime; // 
    // DEF
    // SPD


    void Start(){
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        currentHealth = maxHealth;
        animator.SetBool("isDead", false);
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        Walk();
        Attack();
    }

    void Walk(){
        if (animator.GetFloat("xSpeed") > 0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(2);
            hitboxRange = 1f;
        }

        else if (animator.GetFloat("xSpeed") < -0.01f){
            meleeAttackPoint = this.gameObject.transform.GetChild(1);
            hitboxRange = 1f;
        }
    }

    void Attack(){
        if (Vector3.Distance(transform.position, player.transform.position) < range){
            animator.SetTrigger("slash");
            // Detecting attack range
            Collider2D hitPlayer = Physics2D.OverlapCircle(meleeAttackPoint.position, hitboxRange, playerLayers);
            // Deal damage
            hitPlayer.GetComponent<PlayerMovement>().TakeDamage(attack);
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) 
            return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, hitboxRange);
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
        GetComponent<AIPath>().enabled = false;
        this.enabled = false;
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