using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelSpriter : MonoBehaviour
{
    private Hel master;
    public bool death;
    bool sideAttack;

    Collider2D[] circleHits;

    Collider2D[] innerHits;

    private Collider2D hitPlayer;
    private HelSpawner spawner;


    void Start() {  
        master = this.GetComponentInParent<Hel>();
        master.isAttacking = false;
        death = false;
        spawner = GameObject.FindGameObjectWithTag("HelThrone").GetComponentInChildren<HelSpawner>();
    }

    void Update() {
        if (master.isAttacking) {
            if (sideAttack)
                circleIntersection();
            
            else
                boxIntersection();
        } 
    }

    public void StartAttackSound(){
        master.audio.clip = master.slash;
        master.audio.time = 2.7f;
        master.audio.Play();
    }

    void circleIntersection() {
        if (master.meleeAttackPoint == null || master.meleeAttackCircle == null)
                return;

        
        circleHits = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.targetLayer);
        innerHits = Physics2D.OverlapCircleAll(master.meleeAttackCircle.position, master.meleeInnerRange, master.targetLayer);

        intersection();
    }
    

    void boxIntersection() {
            if (master.meleeAttackPoint == null || master.meleeAttackBox == null)
                return;

            circleHits = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.targetLayer);

            innerHits = Physics2D.OverlapBoxAll(master.meleeAttackBox.position, new Vector2(master.xRange, master.yRange), 0, master.targetLayer);

            intersection();
    }

    private void intersection() {
        // Simle check for collisions in gizmos
            if (circleHits == null || innerHits == null)
                return;

            HashSet<Collider2D> circleCollisions = new HashSet<Collider2D>();
            List<Collider2D> trueCollisions = new List<Collider2D>();

            foreach (Collider2D collider in circleHits)
                circleCollisions.Add(collider);

            foreach (Collider2D collider in innerHits) {
                if (circleCollisions.Contains(collider))
                    trueCollisions.Add(collider);
            }

            // Check if something collided with both gizmos
            if (trueCollisions.Count == 0) 
                return;

            foreach (Collider2D collider in trueCollisions) {
                if (collider.CompareTag("Player")) {
                    master.meleeAttackPoint = null;
                    master.meleeAttackBox = null;
                    master.meleeRange = 0f;
                    master.xRange = 0f;
                    master.yRange = 0f;
                    collider.GetComponent<Player>().TakeDamage(master.attack, "Hel");
                }
            }
    }

    public void startAttack() {
        master.isAttacking = true;
    }

    public void endAttack() {
        master.isAttacking = false;
        master.aiPath.enabled = true;
        sideAttack = false;
    }

    public void startSummon(){
        if (spawner.spawnable){
            master.aiPath.enabled = false;
            spawner.Spawn();
        }
    }

    public void endSummon(){
        master.aiPath.enabled = true;
        spawner.spawnable = true;
    }

    // FIX death animation same as attack

    public void Die() {
        death = true;
    }

    /*          Collider logic
        --------------------------------------
        Draw gizmos for attack range depending 
        on direction
    */
    public void Up() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 2.2f;
        master.meleeAttackBox = master.meleeAttackPoint.GetChild(0);
        master.xRange = 3.7f;
        master.yRange = 1.5f;
    }
    public void Left() {
        sideAttack = true;
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 1.3f;
        master.meleeAttackCircle = master.meleeAttackPoint.GetChild(0);
        master.meleeInnerRange = 1.4f;

    }
    public void Down() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        master.meleeRange = 2.2f;
        master.meleeAttackCircle = master.meleeAttackPoint.GetChild(0);
        master.xRange = 3.5f;
        master.yRange = 1.3f;

    }
    public void Right() {
        sideAttack = true;
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(4);
        master.meleeRange = 1.3f;
        master.meleeAttackCircle = master.meleeAttackPoint.GetChild(0);
        master.meleeInnerRange = 1.4f;
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
            master.Attack();
        
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
            master.aiPath.enabled = true;
    }

}
