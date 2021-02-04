using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : ItemController
{    
    [SerializeField]
    protected float speedReduction = 1f;

    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // Slow down player speed
            playerScript.SlowDown(speedReduction);
            Remove();
        }
    }
}
