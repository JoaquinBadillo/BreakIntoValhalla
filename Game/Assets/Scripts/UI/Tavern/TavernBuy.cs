using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernBuy : MonoBehaviour {
    private GameObject player;
    Player playerScript;
    // Start is called before the first frame update

    public void Buy(int price) {
        GameObject.FindWithTag("Player").GetComponent<Player>().Buy(price);
    }

}
