using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArcherAttack : PlayerAttacker {
    // Melee variables
    private bool isAttacking;
    private Collider2D[] hitEnemies;
    // Shooting variables
    private bool isShooting;
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
                hitEnemies = Physics2D.OverlapCircleAll(Vector2.zero, 0, master.targetLayer);
            else
                hitEnemies = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.targetLayer);
            Debug.Log("This what we got");
            // Deal damage
            foreach(Collider2D enemy in hitEnemies) {
                if (enemy.CompareTag("Saber Draugr")) {
                    Debug.Log("Lo logro señor");
                    master.meleeAttackPoint = null;
                    enemy.GetComponent<MeleeDraugr>().TakeDamage(master.attack);
                }
                else if(enemy.CompareTag("Bow Draugr")) {
                    Debug.Log("Lo logro señor");
                    master.meleeAttackPoint = null;
                    enemy.GetComponent<RangedDraugr>().TakeDamage(master.attack);
                }
                else if (enemy.CompareTag("Hel")) {
                    Debug.Log("Lo logro señor");
                    master.meleeAttackPoint = null;
                    enemy.GetComponent<Hel>().TakeDamage(master.attack);
                }  
                else if(enemy.CompareTag("Arrow")) {
                    Debug.Log("you just got yeeted");
                    base.Deflect(enemy);
                }
            }
        }

        if (isShooting && throwable) {
            GameObject projectile = Instantiate(arrow, master.shootPoint.position, master.shootPoint.rotation);
            projectile.GetComponent<Arrow>().SetAttack(master.secondaryAttack);
            throwable = false;
            Debug.Log("My pointy self just manifested");
            Rigidbody2D projectileRigid2d = projectile.GetComponent<Rigidbody2D>();
            projectileRigid2d.velocity = facing * arrowSpeed;
        }
    }
    public void StartPrimaryAttack() {
        isAttacking = true;
    }

    public void EndPrimaryAttack() {
        isAttacking = false;
    }

    public void StartSecondaryAttack() {
        isShooting = true;
        Physics2D.OverlapCircleAll(Vector2.zero, 0, master.targetLayer);
    }

    public void EndSecondaryAttack(){
        isShooting = false;
        throwable = true;
        UseStamina(master.staminaCost);
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */

    public void Right() {
        base.RightInitializer();
        master.meleeRange = 0.5f;
    }
    public void Left() {
        base.LeftInitializer();
        master.meleeRange = 0.5f;
    }
    public void Up() {
        base.UpInitializer();
        master.meleeRange = 0.35f;
    }
    public void Down() {
        base.DownInitializer();
        master.meleeRange = 0.43f;
    }

    override public void Bless() {
        master.attack += (int) Mathf.Round(master.attack * master.buff);
        master.secondaryAttack += (int) Mathf.Round(master.secondaryAttack * master.buff);
        master.speed -= master.speed * master.debuff;
    }
    override public void DeBless(){
        master.attack -= (int) Mathf.Round(master.attack * master.buff);
        master.secondaryAttack -= (int) Mathf.Round(master.secondaryAttack * master.buff);
        master.speed += master.speed * master.debuff;
    }
}

