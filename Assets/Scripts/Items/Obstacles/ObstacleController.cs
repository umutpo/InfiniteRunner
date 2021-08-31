using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : ItemController
{
    [SerializeField]
    private bool isFatal = false;
    private float speedReduction = 4f;

    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // Slow down player speed
            if (isFatal)
            {
                playerScript.SlowDown(1f);
            }
            else {
                playerScript.SlowDown(speedReduction);
            }
        }
    }
}
