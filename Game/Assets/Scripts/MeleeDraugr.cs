using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeDraugr : MonoBehaviour{
    // Unity Objects
    private Animator animator;
    private Rigidbody2D rigid2d;
    public AIPath aiPath;
    // Animation Variables
    private string currentAnimaton;
    private string direction;
    private string action;
    private bool isAttacking;
    // Attack Variables
    private Transform meleeAttackPoint;
    private float meleeRange;
    public LayerMask enemyLayers;

    void Start(){
        rigid2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    void Update(){
        Walk();
        Idle();
        ChangeAnimationState("draugr-saber-" + direction + action);
    }

    void Walk(){
        // 
        if (aiPath.desiredVelocity.x != 0f || aiPath.desiredVelocity.y != 0f)
            action = "-walk";

        if (aiPath.desiredVelocity.x > 0.01f){
            direction = "-right";
            meleeAttackPoint = this.gameObject.transform.GetChild(4);
            meleeRange = 0.46f;
        }

        else if (aiPath.desiredVelocity.x < 0.01f){
            direction = "-left";
            meleeAttackPoint = this.gameObject.transform.GetChild(2);
            meleeRange = 0.46f;
        }

        else if (aiPath.desiredVelocity.y > 0.01f){
            direction = "-up";
            meleeAttackPoint = this.gameObject.transform.GetChild(1);
            meleeRange = 0.35f;
        }

        else if (aiPath.desiredVelocity.y < 0.01f){
            direction = "-down";
            meleeAttackPoint = this.gameObject.transform.GetChild(3);
            meleeRange = 0.43f;
        }
    }

    void Idle(){
        if (aiPath.desiredVelocity.x == 0f || aiPath.desiredVelocity.y == 0f)
            action = "-idle";
    }

    void PrimaryAttack(){
        if (true){
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

    void ChangeAnimationState(string newAnimation){
        /* 
        This function switches the animations 
        smoothly and returns them upon inputs
        */
        if (currentAnimaton == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}
