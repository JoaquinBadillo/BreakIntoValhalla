using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArcherAttack : MonoBehaviour {
    private Player master;
    // Melee variables
    public bool isAttacking;
    private Collider2D[] hitEnemies;
    // Shooting variables
    public bool isShooting;
    public Transform shootPoint;
    public GameObject arrow;
    public float direction;
    [SerializeField] float arrowSpeed = 20f;
    private Vector3 facing;
    private bool throwable;
    // Deflect variables
    private Vector2 deflectDirection;
    // Stamina variables
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    [SerializeField] GameObject panel;

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
                    //deflectDirection = enemy.GetComponent<DraugrArrow>().facing;
                    enemy.GetComponent<DraugrArrow>().YeetArrow();
                    GameObject projectile = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
                }
            }
        }

        if (isShooting && throwable) {
            GameObject projectile = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
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
        hitEnemies = Physics2D.OverlapCircleAll(master.meleeAttackPoint.position, master.meleeRange, master.targetLayer);
        UseStamina(master.staminaCost);
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */

    public void Right() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(4);
        master.meleeRange = 0.5f;
        direction = 0f;
        facing.x = 1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(8);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Left() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 0.5f;
        direction = 180f;
        facing.x = -1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(6);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Up() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 0.35f;
        direction = 90f;
        facing.x = 0;
        facing.y = 1;
        shootPoint = this.gameObject.transform.parent.GetChild(5);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Down() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        master.meleeRange = 0.43f;
        direction = 270f;
        facing.x = 0;
        facing.y = -1;
        shootPoint = this.gameObject.transform.parent.GetChild(7);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }

    public void UseStamina(int staminaCost) {
        if(master.currentStamina - staminaCost >= 0) {
            master.currentStamina -= staminaCost;
            master.staminaBar.SetValue(master.currentStamina);
            if(regen != null)
                StopCoroutine(regen);
            
            regen = StartCoroutine(RegenStamina());
        }
    }
    
    private IEnumerator RegenStamina() {
        yield return new WaitForSeconds(2f);
        while(master.currentStamina < master.maxStamina) {
            master.currentStamina += master.maxStamina / 100; 
            master.staminaBar.SetValue(master.currentStamina);
            yield return regenTick;
        }
        regen = null; // reset regen
    }

    public void Die() {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine() {
        for (float f = 0f; f <= 1; f += 0.15f) {
            Color c = new Color(0, 0, 0, f);
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.3f);
        }

        SceneManager.LoadScene("DeathScene");
        yield break;
    }
}

