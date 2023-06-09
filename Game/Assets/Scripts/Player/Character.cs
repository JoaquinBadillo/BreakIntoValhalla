using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour{
    // Unity Objects
    public Animator animator; // Unity Object that allows script to change animation states
    protected Rigidbody2D rigid2d; // Unity Object that makes it compatible with physics
    protected SpriteRenderer spriter;
    // Physics
    protected Vector2 movement;
    // Attack Variables
    public Transform meleeAttackPoint; // center of the gizmo
    public float meleeRange; // gizmo radius
    public LayerMask targetLayer; // The layer that can be damaged
    public int childIndex;
    // Stat variables
    // HP
    [SerializeField] protected int maxHealth = 200;
    [SerializeField] protected int currentHealth;
    // ATK
    public int attack = 20;
    public int secondaryAttack = 20;
    // ATKSPD
    public float endLag; // Seconds before next attack can be executed
    public float secondaryEndLag; // Seconds before next attack can be executed
    public float timeUntilNextAttack;
    public float timeUntilNextShot;
    // DEF
    // SPD
    public float speed = 2f;
    
    //Stamina
    public int maxStamina = 100;
    public int currentStamina;

    protected void Initialize(){
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator.SetBool("isDead", false);
    }
    public IEnumerator Flashing() {
        spriter.color = Color.red ;
        yield return new WaitForSeconds(0.1f);
        spriter.color = Color.white;
        yield break;
    }
}