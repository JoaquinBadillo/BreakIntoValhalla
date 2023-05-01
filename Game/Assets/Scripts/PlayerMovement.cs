using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    [SerializeField]
    private float movement_speed = 2f;
    private Animator animator;
    private Rigidbody2D rb2d;
    private float x_axis;
    private float y_axis;
    private string currentAnimaton;
    private string weapon;
    private KeyCode last_hit_key;
    private bool mouse_click;
    private bool is_attacking;
    private bool blessed;
    private bool wounded;
    
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        // Checking for inputs
        x_axis = Input.GetAxisRaw("Horizontal");
        y_axis = Input.GetAxisRaw("Vertical");
        Walk();
        CheckAttack();
    }

    void FixedUpdate(){
        Vector2 vel = new Vector2(0, rb2d.velocity.y);
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
        rb2d.velocity = vel;
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

    string CheckBlessing(){
        string blessed = "";
        return blessed;
    }
    string CheckWound(){
        string wound = "";
        return wound;
    }
    string CheckWeapon(){
        string weapon = "";
        return weapon;
    }
    string CheckDirection(){
        string direction = "";
        return direction;
    }
    string CheckAction(string weapon){
        string action;
        //Attack
        if (weapon == "")
        {
            action = "slash";
        }
        else
        {
            action = "shoot";
        }
        //Walk
        if(x_axis != 0|| y_axis != 0){
            action = "walk";
        }
        //Idle
        if(x_axis != 0|| y_axis != 0){
            action = "walk";
        }
        return action;
    }

   
    void Walk(){
        // ChangeAnimationState();
    }

    void CheckAttack(){
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            mouse_click = true;
        }
    }

    void Attack(){
        if (mouse_click)
        {
            mouse_click = false;
            if(!mouse_click){
                //ChangeAnimationState();
            }
        }
    }


    /*
        KEY DISTRIBUTION
        Input.GetKeyDown(KeyCode.Mouse0) // once
        Input.GetKey(KeyCode.W) // continuous
        Input.GetKey(KeyCode.A) // continuous
        Input.GetKey(KeyCode.S) // continuous
        Input.GetKey(KeyCode.D) // continuous
    */
}
