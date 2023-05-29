using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelSpriter : MonoBehaviour
{
    private Hel master;
    private bool isAttacking;
    public bool death;
    private Collider2D hitPlayer;
    void Start() {  
        master = this.GetComponentInParent<Hel>();
        isAttacking = false;
        death = false;
    }

    void Update() {
        if (isAttacking) {
            // Detecting attack range
            if (master.meleeAttackPoint == null) 
                hitPlayer = Physics2D.OverlapCircle(Vector2.zero, 0, master.playerLayers);
            else
                hitPlayer = Physics2D.OverlapCircle(master.meleeAttackPoint.position, master.meleeRange, master.playerLayers);
            

            // Deal damage
            if (hitPlayer == null) return;

            if(hitPlayer.GetComponent<Player>() != null){
                    master.meleeAttackPoint = null;
                    hitPlayer.GetComponent<Player>().TakeDamage(master.attack);
            }
        }
    }

    public void startAttack() {
        isAttacking = true;
    }

    public void endAttack() {
        isAttacking = false;
    }

    // FIX death animation same as attack

    public void endOfAnimation() {
        death = true;
    }

    //    Draw gizmo for attack range
    public void Up(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 1f;
    }
    public void Left(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 1.55f;
    }

    public void Down() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        master.meleeRange = 1f;
    }
    public void Right(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(4);
        master.meleeRange = 1.55f;
    }

}
