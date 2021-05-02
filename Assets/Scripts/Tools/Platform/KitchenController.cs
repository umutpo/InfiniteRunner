using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenController : PlatformController
{
    private const float KITCHEN_SIZE = 20f;
    private float speedReduction = 4f;

    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        // Remove once out of camera view
        if (player.transform.position.z > transform.position.z + transform.localScale.z * KITCHEN_SIZE)
            Remove();
    }
    

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            // Slow down player speed
            playerScript.SlowDown(speedReduction);
        }
    }
}
