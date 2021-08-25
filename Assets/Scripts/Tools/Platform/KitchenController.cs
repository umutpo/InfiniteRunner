using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenController : PlatformController
{
    private const float KITCHEN_SIZE = 20f;

    private PlayerController playerController = null;

    protected override void Start() {
        base.Start();
        playerController = player.GetComponent<PlayerController>();
    }

    protected override void Update()
    {
        // Remove once out of camera view
        if (player.transform.position.z > transform.position.z + transform.localScale.z * KITCHEN_SIZE + playerController.GetCurrentSpeed())
            Remove();
    }
    

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            // Slow down player speed
            playerScript.SetGameOver();
        }
    }
}
