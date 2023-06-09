using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BerserkerAttack : PlayerAttacker {
    // Melee variables
    private bool isAttacking;
    private Collider2D[] hitEnemies;
    // Shooting variables
    private bool isSecondaryAttacking;
    // Stamina variables
    private Transform axeAttackBox;

    void Start() {
        master = this.GetComponentInParent<Player>();
        isAttacking = false;
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
        else if (isSecondaryAttacking) {
            // Detecting attack range
            if (axeAttackBox == null) 
                hitEnemies = Physics2D.OverlapBoxAll(Vector2.zero, Vector2.zero, master.targetLayer);
            else
                hitEnemies = Physics2D.OverlapBoxAll(axeAttackBox.position, new Vector2(master.xRange, master.yRange), 0, master.targetLayer);
            Debug.Log("This what we got");
            // Deal damage
            foreach(Collider2D enemy in hitEnemies) {
                if (enemy.CompareTag("Saber Draugr")) {
                    Debug.Log("Lo logro señor");
                    axeAttackBox = null;
                    enemy.GetComponent<MeleeDraugr>().TakeDamage(master.attack);
                }
                else if(enemy.CompareTag("Bow Draugr")) {
                    Debug.Log("Lo logro señor");
                    axeAttackBox = null;
                    enemy.GetComponent<RangedDraugr>().TakeDamage(master.attack);
                }
                else if (enemy.CompareTag("Hel")) {
                    Debug.Log("Lo logro señor");
                    axeAttackBox = null;
                    enemy.GetComponent<Hel>().TakeDamage(master.attack);
                }  
                else if(enemy.CompareTag("Arrow")) {
                    Debug.Log("you just got yeeted");
                    base.Deflect(enemy);
                }
            }
        }
    }

    void OnDrawGizmosSelected() {
        if (axeAttackBox != null)
            Gizmos.DrawWireCube(axeAttackBox.position, new Vector3(master.xRange, master.yRange, master.zRange));

    }
    
    public void StartPrimaryAttack() {
        isAttacking = true;
        Physics2D.OverlapBoxAll(Vector2.zero, Vector2.zero, master.targetLayer);
    }

    public void EndPrimaryAttack() {
        isAttacking = false;
    }

    public void StartSecondaryAttack() {
        isSecondaryAttacking = true;
        Physics2D.OverlapCircleAll(Vector2.zero, 0, master.targetLayer);
    }

    public void EndSecondaryAttack(){
        isSecondaryAttacking = false;
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
        master.meleeRange = 0.8f;
        axeAttackBox = this.gameObject.transform.parent.GetChild(12);
        master.xRange = 1.7f;
        master.yRange = 1.5f;
    }
    public void Left() {
        base.LeftInitializer();
        master.meleeRange = 0.8f;
        axeAttackBox = this.gameObject.transform.parent.GetChild(10);
        master.xRange = 1.7f;
        master.yRange = 1.5f;
    }
    public void Up() {
        base.UpInitializer();
        master.meleeRange = 0.65f;
        axeAttackBox = this.gameObject.transform.parent.GetChild(9);
        master.xRange = 2.3f;
        master.yRange = 1.5f;
    }
    public void Down() {
        base.DownInitializer();
        master.meleeRange = 0.75f;
        axeAttackBox = this.gameObject.transform.parent.GetChild(11);
        master.xRange = 2.5f;
        master.yRange = 1.5f;
    }

    override public void Bless() {
        master.attack = (int) Mathf.Round(master.attack * master.buff);
        master.secondaryAttack = (int) Mathf.Round(master.secondaryAttack * master.buff);
        master.speed = master.speed * master.debuff;
    }
    override public void DeBless(){
        master.attack = (int) Mathf.Round(master.attack / master.buff);
        master.secondaryAttack = (int) Mathf.Round(master.secondaryAttack / master.buff);
        master.speed = master.speed / master.debuff;
    }
}

