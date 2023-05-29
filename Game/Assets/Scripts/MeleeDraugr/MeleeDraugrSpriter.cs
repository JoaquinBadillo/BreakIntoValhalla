using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDraugrSpriter : MonoBehaviour
{
    private MeleeDraugr master;
    private bool isAttacking;
    public bool death;
    private Collider2D hitPlayer;
    void Start() {  
        master = this.GetComponentInParent<MeleeDraugr>();
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
                    Debug.Log("Lo logro señor");
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
        master.aiPath.enabled = true;
    }

    // FIX death animation same as attack

    public void endOfAnimation() {
        death = true;
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */
    public void Right(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 1f;
    }
    public void Left(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 1f;
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
            master.Attack();
    }
}
