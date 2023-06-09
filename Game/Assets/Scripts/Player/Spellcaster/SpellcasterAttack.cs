using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpellcasterAttack : PlayerAttacker {
    // Melee variables
    private bool isAttacking;
    private Collider2D[] hitEnemies;
    // Casting variables
    private bool isCasting; // isShooting
    private GameObject spell; // spell
    [SerializeField] float spellSpeed = 10f; // arrow speed
    private bool throwable;
    // Deflect variables
    private Vector2 deflectDirection;

    void Start() {
        master = this.GetComponentInParent<Player>();
        isAttacking = false;
        throwable = true;
    }

    void Update(){
        if (isAttacking) {
            // Detecting attack range
            if (master.meleeAttackPoint == null) 
                hitEnemies = Physics2D.OverlapBoxAll(Vector2.zero, Vector2.zero, master.targetLayer);
            else
                hitEnemies = Physics2D.OverlapBoxAll(master.meleeAttackPoint.position, new Vector2(master.xRange, master.yRange), 0, master.targetLayer);
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

        if (isCasting && throwable) {
            GameObject projectile = Instantiate(spell, master.shootPoint.position, master.shootPoint.rotation);
            projectile.GetComponent<Spell>().SetAttack(master.secondaryAttack);
            throwable = false;
            Debug.Log("My pointy self just manifested");
            Rigidbody2D projectileRigid2d = projectile.GetComponent<Rigidbody2D>();
            projectileRigid2d.velocity = facing * spellSpeed;
        }
    }

    public void StartPrimaryAttack() {
        isAttacking = true;
    }

    public void EndPrimaryAttack() {
        isAttacking = false;
    }

    public void StartSecondaryAttack() {
        isCasting = true;
        hitEnemies = Physics2D.OverlapBoxAll(Vector2.zero, Vector2.zero, master.targetLayer);
    }

    public void EndSecondaryAttack(){
        isCasting = false;
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
        master.xRange = 0.5f;
        master.yRange = 0.3f;
    }
    public void Left() {
        base.LeftInitializer();
        master.xRange = 0.5f;
        master.yRange = 0.3f;
    }
    public void Up() {
        base.UpInitializer();
        master.xRange = 0.3f;
        master.yRange = 1f;
    }
    public void Down() {
        base.DownInitializer();
        master.xRange = 0.3f;
        master.yRange = 0.7f;
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

