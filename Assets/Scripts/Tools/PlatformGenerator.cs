using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    // [SerializeField]
    // protected float initialSpawnRate = 1;
    // protected float spawnRate;
    // protected float lastSpawnTime;

    private int platformCount;
    [SerializeField]
    private int maximumPlatformCount = 8;
    
    private GameObject platform;
    [SerializeField]
    private float spawnDistance = 200;
    private PlatformController platformComponent;
    private Vector3 platPosition;

    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    protected void Start()
    {
        // spawnRate = initialSpawnRate;
        platformCount = 0;
        platPosition = transform.position;
        platform = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, platPosition, Quaternion.identity);
        platPosition.z += platform.transform.localScale.z;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (player.position.z > platPosition.z - spawnDistance && platformCount < maximumPlatformCount)//Time.time > lastSpawnTime + spawnRate && platformCount < maximumPlatformCount)
        {
            // Set position of platform
            platform = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, platPosition, Quaternion.identity);
            platPosition.z += platform.transform.localScale.z;

            platformComponent = platform.GetComponent<PlatformController>();
            platformComponent.OnObjectSpawn();
            platformComponent.onRemovePlatform += RemoveOne;

            platformCount++;
            // lastSpawnTime = Time.time;
        }
    }

    
    void RemoveOne()
    {
        platformCount--;
    }
}
