using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour{
    private Player master;
    private bool isAttacking;

    void Start() {
        master = this.GetComponentInParent<Player>();
        isAttacking = false;
    }

    void Update(){
        if (isAttacking){
            // Detecting attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.enemyLayers);
        // Deal damage
        foreach(Collider2D enemy in hitEnemies)
            enemy.GetComponent<MeleeDraugr>().TakeDamage(master.attack);
        }
    }
    public void startAttack(){
        isAttacking = true;
    }

    public void endAttack(){
        isAttacking = false;
    }
}
