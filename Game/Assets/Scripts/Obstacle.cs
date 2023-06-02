using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroys the obstacle
    public void Yeet() {
        Debug.Log("What's a yeet?");
        Destroy(this.gameObject);
    }
}
