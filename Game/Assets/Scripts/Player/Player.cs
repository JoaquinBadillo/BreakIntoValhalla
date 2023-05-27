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
using UnityEngine.UI;
using TMPro;

public class Player : Character{
    // Health Bar variables
    [SerializeField] TMP_Text hitpoints;
    public SliderMaster healthBar;

    // Sets necessary parameters and gets necessary components
    void Start(){
        attack = 20;
        speed = 2f;
        endLag = 1.5f;
        maxHealth = 200;
        base.Initialize();
        healthBar.SetMaxHealth(maxHealth);
        hitpoints.text = currentHealth + "/" + maxHealth;
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
        if (Time.time >= timeUntilNextAttack)
            PrimaryAttack();
        SecondaryAttack();
    }

    /*
        Function that has a more reliable refresh rate and 
        is commonly used for physics
    */
    void FixedUpdate() {
        rigid2d.MovePosition(rigid2d.position + movement * speed * Time.fixedDeltaTime);
    }

    // This function allows the player to attack on command
    void PrimaryAttack(){
        if (Input.GetKeyDown(KeyCode.P)){
            animator.SetTrigger("primaryAttack");
            timeUntilNextAttack = Time.time + endLag;
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
        Debug.Log("AAAAGH i've been hit");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        hitpoints.text = currentHealth + "/" + maxHealth;
    }

    void Die(){
        animator.SetBool("isDead", true);
        Debug.Log("I died");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public int GetMaxHealth(){
        return maxHealth;
    }
}