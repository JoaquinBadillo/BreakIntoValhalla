using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour{
    private Player master;
    public bool isAttacking;
    private Collider2D[] hitEnemies;

    void Start() {
        master = this.GetComponentInParent<Player>();
        isAttacking = false;
    }

    void Update(){
        if (isAttacking){
            // Detecting attack range
            if (master.meleeAttackPoint == null) 
                hitEnemies = Physics2D.OverlapCircleAll(Vector2.zero, 0, master.enemyLayers);
            else
                hitEnemies = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.enemyLayers);
            Debug.Log("This what we got");
            // Deal damage
            foreach(Collider2D enemy in hitEnemies){
                if(enemy.GetComponent<MeleeDraugr>() != null){
                    Debug.Log("Lo logro señor");
                    master.meleeAttackPoint = null;
                    enemy.GetComponent<MeleeDraugr>().TakeDamage(master.attack);
                }
            }
        }
    }
    public void startAttack(){
        isAttacking = true;
    }

    public void endAttack(){
        isAttacking = false;
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */

    public void Right(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(4);
        master.meleeRange = 0.5f;
    }
    public void Left(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 0.5f;
    }
    public void Up(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 0.35f;
    }
    public void Down(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        master.meleeRange = 0.43f;
    }
}

