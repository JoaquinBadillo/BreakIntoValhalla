/*
    Interactive Abstract Class

    Defines common behaviours for interactive objects.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour {
    protected bool interactable = true;
    protected bool inRange = false;
    protected bool finished = false;

    [SerializeField] protected bool inShop = false;

    [SerializeField] protected GameObject _text = null;

    protected float alpha = 0.7f;

    [SerializeField] protected SpriteRenderer spriter;

    // Interactive objects save the player that interacted with them
    protected Player player;

    protected void Update() {
        if (finished && !inShop) Destroy(this.gameObject);
        alpha = 0.15f * Mathf.Sin(Mathf.PI * Time.time) + 0.85f;

        if (inRange) {
            spriter.color = new Color(1f, 1f, 1f, alpha);

            if (interactable && Input.GetKeyDown(KeyCode.F)) {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // Avoid multiple interaction shenanigans
                interactable = false;
                finished = false;
                Interact();
            }
        }
    }

    // The Interact method is what makes each interactive object unique!
    virtual public void Interact() {}

    // Players interact with objects if they are in range (inside trig.) 
    // and press F

    protected void OnTriggerEnter2D(Collider2D other) {
        if (!interactable || other.gameObject.tag != "Player") return;
        if (_text != null) {
            _text.SetActive(true);
        }

        inRange = true;
    }

    protected void OnTriggerExit2D(Collider2D other) {
        if (!interactable || other.gameObject.tag != "Player") return;
        if (_text != null) {
            _text.SetActive(false);
        }
        spriter.color = new Color(1f, 1f, 1f, 0f);
        inRange = false;
    }
}
