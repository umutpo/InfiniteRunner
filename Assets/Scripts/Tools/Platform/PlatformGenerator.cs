using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private int platformCount;
    [SerializeField]
    private int maximumPlatformCount = 15;
    
    private GameObject platform;
    [SerializeField]
    private float spawnDistance = 300;
    private PlatformController platformComponent;
    private Vector3 platPosition;

    [SerializeField]
    private GameObject player;

    protected void Start()
    {
        platformCount = 0;
        platPosition = transform.position;
        platform = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, platPosition, Quaternion.identity);
        if (platform != null)
        {
            platPosition.z += platform.transform.localScale.z;
        }
    }

    protected void Update()
    {
        float spawnRange = platPosition.z - spawnDistance;
        while (player.transform.position.z > spawnRange && platformCount < maximumPlatformCount)
        {
            // Set position of platform
            platform = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, platPosition, Quaternion.identity);
            // Check if platform spawned
            if (platform == null) return;   // Should never occur if pool size is larger than max
            platPosition.z += platform.transform.localScale.z;

            platformComponent = platform.GetComponent<PlatformController>();
            platformComponent.OnObjectSpawn();
            platformComponent.onRemoveItem += RemoveOne;

            platformCount++;
            spawnRange = platPosition.z - spawnDistance;
        }
    }

    
    void RemoveOne()
    {
        platformCount--;
    }
}
