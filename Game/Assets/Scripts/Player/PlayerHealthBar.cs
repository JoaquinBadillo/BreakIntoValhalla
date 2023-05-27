// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class PlayerHealthBar : MonoBehaviour
// {
//     public int maxHealth = 100;
//     public int currentHealth;
//     [SerializeField] TMP_Text hitpoints;
//     public SliderMaster healthBar;

//     void GameStart()
//     {
//         currentHealth = maxHealth;
//         healthBar.SetMaxHealth(maxHealth);
//         hitpoints.text = currentHealth + "/" + maxHealth;
//     }

// public void TakeDamage(int damage){
//         if (currentHealth <= 0)
//             Die();
        
//         currentHealth -= damage;
//         healthBar.SetHealth(currentHealth);
//         hitpoints.text = currentHealth + "/" + maxHealth;

//         // if (!immune){
//         //     currentHealth -= damage;
//         //     immune = true;
//         // }
//         // else{

//         // }
//     }

//     void Die(){
//         animator.SetBool("isDead", true);
//         Debug.Log("I died");
//         GetComponent<Collider2D>().enabled = false;
//         this.enabled = false;
//     }
// }
