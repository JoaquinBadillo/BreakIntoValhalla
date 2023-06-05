using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryDash : MonoBehaviour
{
    // Dash Variables
    private bool canDash = true;
    private bool isDashing = false;
    private float dashCooldown = 6f;
    private float dashSpeed = 20f;
    private float dashDuration = 0.5f;

    [SerializeField] TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
