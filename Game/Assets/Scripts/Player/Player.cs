/* 
    Player Behavior Script

    Sets the different behaviours of a player

        - Movement (using Vector2)
        - Animations (using the Animator state machine and animator setters)
        - Melee Combat Mechanics (using gizmos for reach)
        - Ranged Combat Mechanics ()
        - Take Damage (using methods called in enemy scripts)
        - Death (deactivating different components with GetComponent<>() method)
    
        Contributed by Pablo Bolio

        - Set player stats from DB, 
        - Upgrade player stats (called by interactive upgrade objects)
        - Heal player (called by potions) 
        - Key (its a hammer visually tho) collection
        - Set current room (called by room script)
        - Set killer (damageDealer data sent by enemies on attack)
        - Minor changes to blessings to enable and disable effects

        Contributed by Joaquin Badillo

        - Stamina mechanics

        Contributed by Shaul Zayat
    
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Stats {
    // HP
    public int hp;
    // Attack Damage for each weapon
    public int primary_attack;
    public int secondary_attack;
    // Attack Speed for each weapon
    public float primary_lag;
    public float secondary_lag;
    // Defense
    public int defense;
    // Movement Speed
    public float speed;
}

public class Player : Character {
    // Unity Components
    private AudioSource audio;
    // Slaves
    private PlayerAttacker spriterSlave;
    private Blessed blessedSlave;
    private Afterimage afterimageSlave;
    // Box Physics Variables
    public float xRange;
    public float yRange;
    public float zRange;
    // Shootpoint
    public Transform shootPoint;
    // Health Bar variables
    [SerializeField] TMP_Text hitpoints;
    public SliderMaster healthBar;
    public SliderMaster staminaBar;
    // Stamina cost
    public int staminaCost = 20;
    private string className;
    private bool keyCollected;
    [SerializeField] private int defense;

    private bool canSetKiller = true;

    // Shop Variables
    [SerializeField] int coins = 200;
    private int BigPotionPrice = 100; // 50% of max health
    private int UpgradePrice = 75; // Upgrade stats
    private int SmallPotionPrice = 25; // 10% of max health
    // Canvas
    GameObject upgradeCanvas;
    GameObject tavernCanvas;
    // Dash Variables
    public Vector3 lastmovementDir;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDistance;
    [SerializeField] LayerMask dashLayer;
    [SerializeField] float timeUntilNextDash;
    public bool isDashing;
    // Blessing Variables
    public float buff;
    public float debuff;
    private float effectDuration = 10f;
    [SerializeField] float timeUntilDeactivation;
    private float blessingCooldown = 40f;
    [SerializeField] float timeUntilNextBlessing;
    public bool isBlessed;
    [SerializeField] HostSO host;
   
    // Sets necessary parameters and gets necessary components
    void Start() {
        StartCoroutine(FetchStats());
        spriterSlave = GetComponentInChildren<PlayerAttacker>();
        blessedSlave = GetComponentInChildren<Blessed>();
        afterimageSlave = GetComponentInChildren<Afterimage>();
        audio = GetComponent<AudioSource>();
        base.Initialize();
        currentStamina = maxStamina;
        staminaBar.SetMaxValue(maxStamina);
        tavernCanvas = GameObject.Find("TavernCanvas");
        upgradeCanvas = GameObject.FindWithTag("Upgrade");
        dashCooldown = 6f;
        dashDistance = 4.2f;
        isDashing = false;
        buff = 1.4f;
        debuff = 0.75f;
        isBlessed = false;
        if(upgradeCanvas != null)
            upgradeCanvas.SetActive(false);

    }

    // Function that is called each frame
    void Update() {
        // Checking for WASD or arrow key inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // Avoids the 1.4 value when moving diagonally
        movement.Normalize();
        // Sets the speed vectors for the animator parameters
        animator.SetFloat("xSpeed", movement.x);
        animator.SetFloat("ySpeed", movement.y);
        if (Time.time >= timeUntilNextAttack)
            PrimaryAttack();

        if (Time.time >= timeUntilNextShot && currentStamina - staminaCost >= 0)
            SecondaryAttack();

        if (movement.x != 0 || movement.y != 0)
            lastmovementDir = movement;

        if (Time.time >= timeUntilNextDash)
                Dash();
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Bless();

        if (isBlessed && Time.time >= timeUntilDeactivation)
            DeBless();
        
        if (isBlessed)
            blessedSlave.AuraOn();
        else
            blessedSlave.AuraOff();
        // Gotta love them if statements
    }

    /*
        Function that has a more reliable refresh rate and 
        is commonly used for physics
    */
    void FixedUpdate() {
        rigid2d.velocity = movement * speed;
        if (isDashing){
            Vector3 dashPosition = transform.position + lastmovementDir * dashDistance;

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastmovementDir, dashDistance, dashLayer);
            if (raycastHit.collider != null) {
                dashPosition = raycastHit.point;
            }
            rigid2d.MovePosition(dashPosition);
            isDashing = false;
        }
    }

    // This function allows the player to attack on command
    void PrimaryAttack() {
        if (Input.GetKeyDown(KeyCode.P)) {
            animator.SetTrigger("primaryAttack");
            timeUntilNextAttack = Time.time + endLag;
        }
    }

    void OnDrawGizmosSelected() {
        if (meleeAttackPoint == null) 
            return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
    }

    void SecondaryAttack() {
        if (Input.GetKeyDown(KeyCode.L)) {
            animator.SetTrigger("secondaryAttack");
            timeUntilNextShot = Time.time + secondaryEndLag;
        }
    }

    public void TakeDamage(int damage, string damageSource) {
        StartCoroutine(Flashing());
        damage -= defense;

        if (damage < 2)
        currentHealth -= 2;
        else
            currentHealth -= damage;

        healthBar.SetValue(currentHealth);
        if (currentHealth <= 0)
            Die(damageSource);
        else  
            hitpoints.text = currentHealth + "/" + maxHealth;
    }

    void Die(string damageSource) {
        // Avoid some enemy kill-steal shenanigans
        if (canSetKiller) {
            spriterSlave.killer = damageSource;
            canSetKiller = false;
        }

        hitpoints.text = "0 /" + maxHealth;
        animator.SetBool("isDead", true);
    }

    public void SetStats(Stats stats) {
        maxHealth = stats.hp;
        currentHealth = stats.hp;
        attack = stats.primary_attack;
        secondaryAttack = stats.secondary_attack;
        endLag = stats.primary_lag;
        secondaryEndLag = stats.secondary_lag;
        defense = stats.defense;
        speed = stats.speed;
    }

    public void Buy(int price) {
        if(coins - price >= 0){
            coins -= price;
            if (price == BigPotionPrice)
                    Heal(0.5f);

            else if (price == UpgradePrice)
                    upgradeCanvas.SetActive(true);

            else if (price == SmallPotionPrice)          
                    Heal(0.1f);
            audio.Play();
        }       

    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public void UpgradeAttack() {
        attack += 3;
    }

    public void UpgradeHealth() {
        maxHealth += 20;
        currentHealth += 20;
        // Update health bar
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetValue(currentHealth);
        hitpoints.text = currentHealth + "/" + maxHealth;
    }

    public void UpgradeSpeed() {
        speed += 0.4f;
    }

    public void Heal(float healingRate) {
        currentHealth += (int) (maxHealth * healingRate);
        
        // Avoid over-healing shenaningans
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        
        // Update health bar
        healthBar.SetValue(currentHealth);
        hitpoints.text = currentHealth + "/" + maxHealth;
    }

    public void SetKeyStatus(bool status) {
        keyCollected = status;
    }

    public bool HasKey() {
        return keyCollected;
    }

    void Dash() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isDashing = true;
            afterimageSlave.ActivateAfterimage();
            timeUntilNextDash = Time.time + dashCooldown;
        }
    }

    // This function allows the player to get blessed on command
    void Bless() {
        if (timeUntilDeactivation >= Time.time || timeUntilNextBlessing >= Time.time)
            return;
        

        spriterSlave.Bless();
        isBlessed = true;
        timeUntilDeactivation = Time.time + effectDuration;
    }
    void DeBless(){

        spriterSlave.DeBless(); 
        timeUntilNextBlessing = Time.time + blessingCooldown;
        isBlessed = false;
    }

    public void SetCurrentRoom(string currentRoom) {
        spriterSlave.currentRoom = currentRoom;
    }

    public void AddKill() {
        spriterSlave.kills++;
    }

    public void AddCoins(int _coins) {
        coins += _coins;
    }

    public int GetKills() {
        return spriterSlave.kills;
    }

    public IEnumerator FetchStats() {
        string endpoint = host.uri + "characters/" + PlayerPrefs.GetInt("classIndex") + "/stats";
        Debug.Log(endpoint);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check if fetching was successful and send json to callback
            if (webRequest.result == UnityWebRequest.Result.Success) {
                string jsonString = webRequest.downloadHandler.text;
                Debug.Log(jsonString);
                Stats stats = JsonUtility.FromJson<Stats>(webRequest.downloadHandler.text);
                SetStats(stats);   
            }

            else {
                Debug.Log("Make sure to start the server!");
                Debug.Log("Error: " + webRequest.error);
                // Set Default Stats
                maxHealth = 200;
                attack = 20;
                secondaryAttack = 15;
                endLag = 1.5f;
                secondaryEndLag = 1.5f;
                speed = 2f;
            }

            healthBar.SetMaxValue(maxHealth);
            hitpoints.text = currentHealth + "/" + maxHealth;
        }
    }
}