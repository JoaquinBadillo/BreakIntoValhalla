using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeDraugrSpriter : MonoBehaviour
{
    private MeleeDraugr master;
    private bool isAttacking;
    public bool death;
    private Collider2D hitPlayer;

    [SerializeField] int coins;
    void Start() {  
        master = this.GetComponentInParent<MeleeDraugr>();
        isAttacking = false;
        death = false;
    }

    void Update() {
        if (isAttacking) {
            // Detecting attack range
            if (master.meleeAttackPoint == null) 
                hitPlayer = Physics2D.OverlapCircle(Vector2.zero, 0, master.targetLayer);
            else
                hitPlayer = Physics2D.OverlapCircle(master.meleeAttackPoint.position, master.meleeRange, master.targetLayer);
            // Deal damage
            if (hitPlayer == null) return;
            if(hitPlayer.GetComponent<Player>() != null){
                    Debug.Log("Lo logro señor");
                    master.meleeAttackPoint = null;
                    hitPlayer.GetComponent<Player>().TakeDamage(master.attack, "Sword Draugr");
                }
        }
    }

    public void startAttack() {
        isAttacking = true;
    }

    public void endAttack() {
        isAttacking = false;
        master.aiPath.enabled = true;
        master.canAttack = true;
    }

    public void StartAttackSound(){
        master.audio.clip = master.slash;
        master.audio.time = 2.7f;
        master.audio.Play();
    }

    public void StartDeath() {
        master.meleeAttackPoint = null;
        this.GetComponentInParent<Seeker>().enabled = false;
        this.GetComponentInParent<AIPath>().enabled = false;
        this.GetComponentInParent<AIDestinationSetter>().enabled = false;
        this.GetComponentInParent<Collider2D>().enabled = false;
    }

    public void EndDeath() {
        death = true;
        StartCoroutine(WaitToDie());
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
        master.animator.SetBool("lastRight", true);
        master.animator.SetBool("lastLeft", false);
    }
    public void Left(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 1f;
        master.animator.SetBool("lastLeft", true);
        master.animator.SetBool("lastRight", false);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
            master.Attack();
    }

    public IEnumerator WaitToDie() {
        master.GetComponent<Collider2D>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddKill();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddCoins(coins);
        yield return new WaitForSeconds(1f);
        Destroy(master.gameObject);
    }
}
