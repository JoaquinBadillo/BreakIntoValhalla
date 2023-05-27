using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDraugrSpriter : MonoBehaviour
{
    private MeleeDraugr master;
    private bool isAttacking;
    void Start() {  
        master = this.GetComponentInParent<MeleeDraugr>();
        isAttacking = false;
    }

    void Update() {
        if (isAttacking) {
            // Detecting attack range
            Collider2D hitPlayer = Physics2D.OverlapCircle(master.meleeAttackPoint.position, master.meleeRange, master.playerLayers);
            // Deal damage
            hitPlayer.GetComponent<Player>().TakeDamage(master.attack);
        }
    }

    public void startAttack() {
        isAttacking = true;
    }

    public void endAttack() {
        isAttacking = false;
    }

    public bool endOfAnimation() {
        return true;
    }
}
