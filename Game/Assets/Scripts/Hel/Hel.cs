using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;
using UnityEngine.SceneManagement;
public class Hel : Character{
    private Blessed auraSlave;
    // Movement Variables
    public AIPath aiPath;
    [SerializeField] Transform player;
    // Attack Variables
    private HelSpriter animatorSlave;
    public float meleeInnerRange;
    public float xRange;
    public float yRange;
    public float zRange;
    public Transform meleeAttackBox;
    public Transform meleeAttackCircle;
    public bool isAttacking;
    // Health Bar Variables
    [SerializeField] TMP_Text hitpoints;
    public HelSlider healthBar;
    // Second Phase Variables
    private bool isSecondPhase;
    private float timeUntilNextSummon;
    private float summonLag;
    private int randomizedSummon;

    void Start() {
        maxHealth = 600;
        endLag = 5f;
        summonLag = 25f;
        meleeAttackBox = null;
        meleeAttackCircle = null;
        isSecondPhase = false;
        zRange = 1;
        auraSlave = this.GetComponentInChildren<Blessed>();
        base.Initialize();
        healthBar = GetComponentInChildren<HelSlider>();
        healthBar.SetMaxHealth(maxHealth);
        animatorSlave = this.gameObject.transform.GetChild(0).GetComponent<HelSpriter>();
        GetComponent<AIPath>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        animator.SetFloat("xSpeed", aiPath.desiredVelocity.x);
        animator.SetFloat("ySpeed", aiPath.desiredVelocity.y);
        if (isSecondPhase) {
            auraSlave.AuraOn();
            Summon();
        }
    }

    public void Attack() {
        if (Time.time >= timeUntilNextAttack){
            aiPath.enabled = false;
            animator.SetTrigger("slash");
            timeUntilNextAttack = Time.time + endLag;
        }
    }

    public void Summon() {
        if (Time.time >= timeUntilNextSummon){
            aiPath.enabled = false;
            animator.SetTrigger("summon");
            timeUntilNextSummon = Time.time + summonLag;
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) 
            return;
        
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);

        if (meleeAttackBox != null)
            Gizmos.DrawWireCube(meleeAttackBox.position, new Vector3(xRange, yRange, zRange));
        
        if (meleeAttackCircle != null)
            Gizmos.DrawWireSphere(meleeAttackCircle.position, meleeRange);

    }

    public void TakeDamage(int damage) {
        StartCoroutine(Flashing());
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 300 && currentHealth > 0){
            isSecondPhase = true;
            animator.SetBool("isSecond", true);
        }

        if (currentHealth <= 0)
            Die();
            
    }

    void Die() {
        // Weird stuff needs to happen here...
        // Hel doesn't really die, cinematic happens
        Debug.Log("I died...");
        animator.SetBool("isDead", true);
        Debug.Log("SIKE!");

        StartCoroutine(Victory());

        Debug.Log("Yeets player out of existence");
        if (animatorSlave.death == true)
            Destroy(gameObject);
         
    }

    IEnumerator Victory() {
        int kills = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetKills() + 1;
        PlayerPrefs.SetInt("kills", kills);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddKill();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("CreditsScene");
    }
}