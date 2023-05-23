using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance; // how far away the enemy will retreat from the player

    private float timeBtwShots; // time between shots
    public float startTimeBtwShots; // time between shots at the start of the game
    public GameObject projectile; // reference to the projectile
    public Transform player; // reference to the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // find the player
        timeBtwShots = startTimeBtwShots; 

    }

    // Update is called once per frame
    void Update()
    {
        // if the enemy is too far from the player
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        // if the enemy is too close to the player
        else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position; // stop moving
        }
        // the enemy is too close to the player and needs to retreat
        else if(Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime); // move away from the player
        }


        if(timeBtwShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity); // create a projectile at the enemy's position
            timeBtwShots = startTimeBtwShots; 
        }
        else 
        {
            timeBtwShots -= Time.deltaTime; // decrease the time between shots
        }
    }
}
