using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    protected float initialSpawnRate = 1;
    protected float spawnRate;
    protected float lastSpawnTime;

    private int platformCount;
    [SerializeField]
    private int maximumPlatformCount = 8;
    
    private GameObject platform;
    private PlatformController platformComponent;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    protected void Start()
    {
        spawnRate = initialSpawnRate;
        platformCount = 1;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Time.time > lastSpawnTime + spawnRate && platformCount < maximumPlatformCount)
        {
            // Set position of platform
            lastPosition.z += 10;   // TODO: change to variable based on gameObject?
            platform = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, lastPosition, Quaternion.identity);

            platformComponent = platform.GetComponent<PlatformController>();
            platformComponent.OnObjectSpawn();
            platformComponent.onRemovePlatform += RemoveOne;

            platformCount++;
            lastSpawnTime = Time.time;
        }
    }

    
    void RemoveOne()
    {
        platformCount--;
    }
}
