using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernBuy : MonoBehaviour
{
    private GameObject player;
    Player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }

    public void Buy(int price)
    {
        playerScript.Buy(price);
    }

}
