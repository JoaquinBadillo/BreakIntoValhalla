using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BerserkerAttack : MonoBehaviour {
    private Player master;
    // Melee variables
    public bool isAttacking;
    private Collider2D[] hitEnemies;
    // Shooting variables
    public bool isSecondaryAttacking;
    public Transform shootPoint;
    public GameObject arrow;
    public float direction;
    private Vector3 facing;
    // Deflect variables
    private Vector2 deflectDirection;
    // Stamina variables
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;
    [SerializeField] GameObject panel;
    public Transform axeAttackBox;

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
                    //deflectDirection = enemy.GetComponent<DraugrArrow>().facing;
                    enemy.GetComponent<DraugrArrow>().YeetArrow();
                    GameObject projectile = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
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
                    //deflectDirection = enemy.GetComponent<DraugrArrow>().facing;
                    enemy.GetComponent<DraugrArrow>().YeetArrow();
                    GameObject projectile = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
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
        master.meleeRange = 0.8f;
        direction = 0f;
        facing.x = 1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(12);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
        axeAttackBox = this.gameObject.transform.parent.GetChild(8);
        master.xRange = 1.7f;
        master.yRange = 1.5f;
    }
    public void Left() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        master.meleeRange = 0.8f;
        direction = 180f;
        facing.x = -1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(10);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
        axeAttackBox = this.gameObject.transform.parent.GetChild(6);
        master.xRange = 1.7f;
        master.yRange = 1.5f;
    }
    public void Up() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        master.meleeRange = 0.65f;
        direction = 90f;
        facing.x = 0;
        facing.y = 1;
        shootPoint = this.gameObject.transform.parent.GetChild(9);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
        axeAttackBox = this.gameObject.transform.parent.GetChild(5);
        master.xRange = 2.3f;
        master.yRange = 1.5f;
    }
    public void Down() {
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        master.meleeRange = 0.75f;
        direction = 270f;
        facing.x = 0;
        facing.y = -1;
        shootPoint = this.gameObject.transform.parent.GetChild(11);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
        axeAttackBox = this.gameObject.transform.parent.GetChild(7);
        master.xRange = 2.5f;
        master.yRange = 1.5f;
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

