using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTrack : MonoBehaviour
{
    [SerializeField] float speed;
    public bool chase = false;
    public Transform startingPoint;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) //Player doesn't exist
        return;
        if(chase == true){
            Chase();
        }
        else{
            ReturnStartPoin();
            //Go to starting position
        }
        Chase();
        Flip();
    }

    private void ReturnStartPoin()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }
    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if(transform.position.x > player.transform.position.x){
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else{
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
