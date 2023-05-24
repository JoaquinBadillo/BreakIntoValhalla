using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y); //target is the player position when the projectile is created
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime); //moves the projectile towards the player

        if (transform.position.x == target.x && transform.position.y == target.y) //if the projectile reaches the player, it is destroyed
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D other) //if the projectile collides with the player, it is destroyed
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectile();
        }
    }
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
