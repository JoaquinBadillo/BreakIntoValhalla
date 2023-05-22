using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    // Unity Objects
    private Animator animator;
    private Rigidbody2D rigid2d;
    // Physics
    [SerializeField] float movement_speed = 2f;
    private Vector2 movement;
    // Attack Variables
    private Transform meleeAttackPoint;
    [SerializeField] float meleeRange;
    [SerializeField] LayerMask enemyLayers;
    // Stat variables
    public int attack = 20;
    
    void Start(){
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    void Update(){
        // Checking for WASD or arrow key inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("xSpeed", movement.x);
        animator.SetFloat("ySpeed", movement.y);
        Walk();
        PrimaryAttack();
        SecondaryAttack();
    }

    void FixedUpdate() {
        rigid2d.MovePosition(rigid2d.position + movement * movement_speed * Time.fixedDeltaTime);
    }

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

    void PrimaryAttack(){
        if (Input.GetKeyDown(KeyCode.P)){
            animator.SetTrigger("primaryAttack");
            // Detecting attack range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeRange, enemyLayers);
            // Deal damage
            foreach(Collider2D enemy in hitEnemies){
                enemy.GetComponent<MeleeDraugr>().TakeDamage(attack);
                Debug.Log("We be slashin" + enemy.name);
            }
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
}