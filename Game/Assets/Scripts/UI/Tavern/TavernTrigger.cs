using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernTrigger : MonoBehaviour
{

    //Recieve the Canvas object
    [SerializeField] GameObject canvasObject;

    // Start is called before the first frame update
    void Start()
    {
        //disable the canvas
        canvasObject.SetActive(false);
    }

    //enter into trigger and appear canvas
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvasObject.SetActive(true);
        }
    }

    //exit trigger and disable canvas
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvasObject.SetActive(false);
        }
    }


}
