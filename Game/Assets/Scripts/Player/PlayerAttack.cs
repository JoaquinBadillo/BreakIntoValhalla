using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private Player master;
    // Melee variables
    public bool isAttacking;
    public bool canAttack;
    private Collider2D[] hitEnemies;
    // Shooting variables
    public bool isShooting;
    public Transform shootPoint;
    public GameObject arrow;
    public float direction;
    [SerializeField] float arrowSpeed = 20f;
    private Vector3 facing;
    private bool throwable;

    void Start() {
        master = this.GetComponentInParent<Player>();
        isAttacking = false;
        throwable = true;
    }

    void Update(){
        if (isAttacking) {
            // Detecting attack range
            if (master.meleeAttackPoint == null) 
                hitEnemies = Physics2D.OverlapCircleAll(Vector2.zero, 0, master.enemyLayers);
            else
                hitEnemies = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.enemyLayers);
            Debug.Log("This what we got");
            // Deal damage
            foreach(Collider2D enemy in hitEnemies) {
                if(enemy.GetComponent<MeleeDraugr>() != null) {
                    Debug.Log("Lo logro señor");
                    master.meleeAttackPoint = null;
                    enemy.GetComponent<MeleeDraugr>().TakeDamage(master.attack);
                }
            }
        }

        if (isShooting && throwable) {
            GameObject projectile = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<Arrow>().SetAttack(master.secondaryAttack);
            throwable = false;
            Debug.Log("My pointy self just manifested");
            Rigidbody2D projectileRigid2d = projectile.GetComponent<Rigidbody2D>();
            projectileRigid2d.velocity = facing * arrowSpeed;

            // arrow.GetComponent<Arrow>().SetAttack(master.secondaryAttack);
        }
    }
    public void startAttack() {
        isAttacking = true;
    }

    public void endAttack() {
        isAttacking = false;
        // canAttack = false;
    }

    public void ShootStart() {
        isShooting = true;
        Physics2D.OverlapCircleAll(Vector2.zero, 0, master.enemyLayers);
    }

    public void ShootFinish(){
        isShooting = false;
        throwable = true;
        hitEnemies = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.enemyLayers);
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */

    public void Right() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(4);
        master.meleeRange = 0.5f;
        direction = 0f;
        facing.x = 1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(8);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Left() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 0.5f;
        direction = 180f;
        facing.x = -1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(6);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Up() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 0.35f;
        direction = 90f;
        facing.x = 0;
        facing.y = 1;
        shootPoint = this.gameObject.transform.parent.GetChild(5);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Down() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        master.meleeRange = 0.43f;
        direction = 270f;
        facing.x = 0;
        facing.y = -1;
        shootPoint = this.gameObject.transform.parent.GetChild(7);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
}

