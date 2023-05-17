using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    // Unity Objects
    private Animator animator;
    private Rigidbody2D rigid2d;
    // Physics
    [SerializeField] float movement_speed = 2f;
    private float x_axis;
    private float y_axis;
    // Animation Variables
    private string currentAnimaton;
    private string direction;
    private string action;
    private bool isPrimaryAttack;
    private bool isSecondaryAttack;
    // Attack Variables
    private Transform meleeAttackPoint;
    private float meleeRange;
    public LayerMask enemyLayers;
    
    void Start(){
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        direction = "-down";
        action = "-idle";
    }

    void Update(){
        // Checking for WASD or arrow key inputs
        x_axis = Input.GetAxisRaw("Horizontal");
        y_axis = Input.GetAxisRaw("Vertical");
        PrimaryAttack();
        SecondaryAttack();
        Walk();
        Idle();
        ChangeAnimationState("archer-female" + direction + action);
    }
    
    void FixedUpdate(){
        /* 
        This function updates more frequently than Update() 
        for reliable physics
        */
        Vector2 vel = new Vector2(0, rigid2d.velocity.y);
        // Movement in x
        if (x_axis < 0){
           vel.x = -movement_speed;
        }
        else if (x_axis > 0){
           vel.x = movement_speed;
        }
        else{
            vel.x = 0;

        }
        // Movement in y
        if (y_axis < 0){
           vel.y = -movement_speed;
        }
        else if (y_axis > 0){
           vel.y = movement_speed;
        }
        else{
            vel.y = 0;
        }
        rigid2d.velocity = vel.normalized * 2;
    }

    void ChangeAnimationState(string newAnimation){
        /* 
        This function switches the animations 
        smoothly and returns them upon inputs
        */
        if (currentAnimaton == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void Walk(){
        // 
        if (x_axis != 0 || y_axis != 0)
            action = "-walk";

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            direction = "-right";
            meleeAttackPoint = this.gameObject.transform.GetChild(4);
            meleeRange = 0.46f;
        }

        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            direction = "-left";
            meleeAttackPoint = this.gameObject.transform.GetChild(2);
            meleeRange = 0.46f;
        }

        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            direction = "-up";
            meleeAttackPoint = this.gameObject.transform.GetChild(1);
            meleeRange = 0.35f;
        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            direction = "-down";
            meleeAttackPoint = this.gameObject.transform.GetChild(3);
            meleeRange = 0.43f;
        }
    }

    void Idle(){
        if (x_axis == 0 && y_axis == 0)
            action = "-idle";
    }

    void PrimaryAttack(){
        if (Input.GetKeyDown(KeyCode.P)){
            // Changing the animation through string concatenation
            action = "-slash";
            ChangeAnimationState("archer-female" + direction + action);
            // Detecting attack range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeRange, enemyLayers);
            // Deal damage
            foreach(Collider2D enemy in hitEnemies)
                Debug.Log("We be slashin" + enemy.name);
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) return;

        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
    }

    void SecondaryAttack(){
        if (Input.GetKeyDown(KeyCode.L)){
            action = "-shoot";
            ChangeAnimationState("archer-female" + direction + action);
        }
    }
}
