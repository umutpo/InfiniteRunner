﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : ItemController
{    
    private float speedReduction = 4f;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            // Slow down player speed
            playerScript.SlowDown(speedReduction);
        }
    }
}
