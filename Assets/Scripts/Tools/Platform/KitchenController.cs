﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenController : PlatformController
{
    private const float KITCHEN_SIZE = 20f;

    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        // Remove once out of camera view
        if (player.transform.position.z > transform.position.z + transform.localScale.z * KITCHEN_SIZE)
            Remove();
    }
}
