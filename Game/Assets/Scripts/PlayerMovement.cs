/* 
    Player Behavior Script

    sets the different behaviours of a player
        - Movement (using Vector2)
        - Animations (using the Animator state machine and animator setters)
        - Melee Combat Mechanics (using gizmos for reach)
        - Ranged Combat Mechanics ()
        - Take Damage (using methods called in enemy scripts)
        - Death (deactivating different components with GetComponent<>() method)

    Pablo Bolio
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    // Unity Objects
    private Animator animator; // Unity Object that allows script to change animation states
    private Rigidbody2D rigid2d; // Unity Object that makes it compatible with physics
    // Physics
    private Vector2 movement;
    // Attack Variables
    private Transform meleeAttackPoint; // center of the gizmo
    [SerializeField] float meleeRange; // gizmo radius
    [SerializeField] LayerMask enemyLayers; // Layer that can be attacked
    private bool immune = false;
    // Stat variables
    // HP
    private int maxHealth = 200;
    private int currentHealth;
    // ATK
    public int attack = 20;
    // ATKSPD
    public float attackRate; // how many attacks per second
    public float nextAttackTime; // number of seconds until player is allowed to attack again
    // DEF
    // SPD
    [SerializeField] float movement_speed = 2f;

    private Stats stats;
    
    // Sets necessary parameters and gets necessary components
    void Start() {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    // Function that is called each frame
    void Update(){
        // Checking for WASD or arrow key inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // Avoids the 1.4 value when moving diagonally
        movement.Normalize();
        // Sets the speed vectors for the animator parameters
        animator.SetFloat("xSpeed", movement.x);
        animator.SetFloat("ySpeed", movement.y);
        Walk();
        if (true)
        {
            PrimaryAttack();
        }
        SecondaryAttack();
    }

    /*
        Function that has a more reliable refresh rate and 
        is commonly used for physics
    */
    void FixedUpdate() {
        rigid2d.MovePosition(rigid2d.position + movement * movement_speed * Time.fixedDeltaTime);
    }

    /*
        This fucntion changes the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */
    void Walk(){
        if (movement.x > 0){
            meleeAttackPoint = this.gameObject.transform.GetChild(4);
            meleeRange = 0.5f; 
        }

        else if (movement.x < 0){
            meleeAttackPoint = this.gameObject.transform.GetChild(2);
            meleeRange = 0.5f;
        }

        else if (movement.y > 0){
            meleeAttackPoint = this.gameObject.transform.GetChild(1);
            meleeRange = 0.35f;
        }

        else if (movement.y < 0){
            meleeAttackPoint = this.gameObject.transform.GetChild(3);
            meleeRange = 0.43f;
        }
    }

    // This function allows the player to attack on command
    void PrimaryAttack(){
        if (Input.GetKeyDown(KeyCode.P)){
            animator.SetTrigger("primaryAttack");
            // Detecting attack range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeRange, enemyLayers);
            // Deal damage
            foreach(Collider2D enemy in hitEnemies)
                enemy.GetComponent<MeleeDraugr>().TakeDamage(attack);
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) 
            return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
    }

    void SecondaryAttack(){
        if (Input.GetKeyDown(KeyCode.L)){
            animator.SetTrigger("secondaryAttack");
        }
    }

    public void TakeDamage(int damage){
        
        if (!immune){
            currentHealth -= damage;
            immune = true;
        }
        else{

        }
        
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