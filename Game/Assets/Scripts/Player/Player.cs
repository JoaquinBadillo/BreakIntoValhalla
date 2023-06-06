/* 
    Player Behavior Script

    Sets the different behaviours of a player

        - Movement (using Vector2)
        - Animations (using the Animator state machine and animator setters)
        - Melee Combat Mechanics (using gizmos for reach)
        - Ranged Combat Mechanics ()
        - Take Damage (using methods called in enemy scripts)
        - Death (deactivating different components with GetComponent<>() method)
    
    Pablo Bolio

        - Set player stats from DB, 
        - Upgrade player stats (called by interactive upgrade objects)
        - Heal player (called by potions) 
        
    Joaquin Badillo

    Shaul Zayat
        - Stamina mechanics
    
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
    // Box Physics Variables
    public float xRange;
    public float yRange;
    // Health Bar variables
    [SerializeField] TMP_Text hitpoints;
    public SliderMaster healthBar;
    public SliderMaster staminaBar;
    // Stamina cost
    public int staminaCost = 20;
    private string className;
    private bool keyCollected;
    private int defense;

    // Coins
    private int coins = 1000;
   
    // Sets necessary parameters and gets necessary components
    void Start() {
        PlayerPrefs.SetInt("classIndex", 1);

        try {
            StartCoroutine(FetchStats());
        }
        
        catch (System.Exception) {
            // If FetchStats fails, use some default values
            Debug.Log("Make sure to start the server!");
            maxHealth = 200;
            attack = 20;
            secondaryAttack = 15;
            endLag = 1.5f;
            secondaryEndLag = 1.5f;
            speed = 2f;
        }

        base.Initialize();
        healthBar.SetMaxValue(maxHealth);
        currentStamina = maxStamina;
        staminaBar.SetMaxValue(maxStamina);
        hitpoints.text = currentHealth + "/" + maxHealth;
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
    }

    /*
        Function that has a more reliable refresh rate and 
        is commonly used for physics
    */
    void FixedUpdate() {
        rigid2d.MovePosition(rigid2d.position + movement * speed * Time.fixedDeltaTime);
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

    public void TakeDamage(int damage) {
        Debug.Log("AAAAGH i've been hit");
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);
        hitpoints.text = currentHealth + "/" + maxHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die() {
        animator.SetBool("isDead", true);
    }

    public void SetStats(Stats stats) {
        maxHealth = stats.hp;
        attack = stats.primary_attack;
        secondaryAttack = stats.secondary_attack;
        endLag = stats.primary_lag;
        secondaryEndLag = stats.secondary_lag;
        defense = stats.defense;
        speed = stats.speed;
    }

    public void Buy(int price) {
        if(coins - price >= 0) 
            coins -= price;
        else 
            Debug.Log("Not enough coins");
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

    public IEnumerator FetchStats() {
        string uri = "http://localhost:5000/api/characters/";
        string endpoint = uri + PlayerPrefs.GetInt("classIndex") + "/stats";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check if fetching was successful and send json to callback
            if (webRequest.result == UnityWebRequest.Result.Success) {
                string jsonString = webRequest.downloadHandler.text;
                Stats stats = JsonUtility.FromJson<Stats>(webRequest.downloadHandler.text);
                SetStats(stats);   
            }

            else {
                Debug.Log("Error: " + webRequest.error);
                throw new System.Exception();
            }
        }
    }
}
