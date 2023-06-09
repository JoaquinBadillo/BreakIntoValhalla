/*
    Hel Script
    - Handles Hel's movement, attacks, and health
    - Hel has two phases, the first phase is a melee attack
    - The second phase is a summoning phase
    - Hel has a health bar that is displayed on the screen
    - When Hel dies the player wins the game

    Pablo Bolio, Joaquin Badillo
*/

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
    [SerializeField] GameObject panel;
    // Audio Variables
    private BackgroundMusic musicMaster;
    public AudioSource audio;
    public AudioClip slash;

    void Start() {
        maxHealth = 1250;
        endLag = 3f;
        summonLag = 10f;
        attack = 60;
        meleeAttackBox = null;
        meleeAttackCircle = null;
        isSecondPhase = false;
        zRange = 1;
        auraSlave = this.GetComponentInChildren<Blessed>();
        audio = GetComponent<AudioSource>();
        musicMaster = GameObject.FindGameObjectWithTag("Sounder").GetComponent<BackgroundMusic>();
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
        if (isSecondPhase)
            Summon();
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

        if (currentHealth <= 300 && currentHealth > 0 && !isSecondPhase){
            isSecondPhase = true;
            auraSlave.AuraOn();
            attack = 70;
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

        GameObject[] draugrs = GameObject.FindGameObjectsWithTag("Bow Draugr");

        foreach (GameObject draugr in draugrs)
            draugr.GetComponent<RangedDraugr>().TakeDamage(1000);

        StartCoroutine(Victory());

        Debug.Log("Yeets player out of existence");
        if (animatorSlave.death == true)
            Destroy(gameObject);
         
    }

    IEnumerator Victory() {
        int kills = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetKills() + 1;
        PlayerPrefs.SetInt("kills", kills);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddKill();

        for (float f = 1f; f >= 0; f -= 0.05f) {
            musicMaster.audio.volume = f;
            yield return new WaitForSeconds(0.05f);
        }
        musicMaster.audio.Stop();
        musicMaster.audio.clip = musicMaster.berserkir;

        // Fade Out
        for (float f = 0f; f <= 1; f += 0.05f) {
            Color c = new Color(0, 0, 0, f);
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("CreditsScene");
    }
}