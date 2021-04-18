using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenController : PlatformController
{
    public float kitchenSize;

    protected override void Start() {
        base.Start();
        // TODO: Update once model is in
        kitchenSize = 20f;
    }

    protected override void Update()
    {
        // Remove once out of camera view
        if (player.transform.position.z > transform.position.z + transform.localScale.z * kitchenSize)
            Remove();
    }
}
