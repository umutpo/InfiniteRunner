using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private int obstacleCount;
    [SerializeField]
    private int maximumObstacleCount = 8;
    
    private GameObject obstacle;
    [SerializeField]
    private float spawnDistance = 200;
    private PlatformController obstacleComponent;
    private Vector3 obstaclePosition;
    const float laneWidth = 3.0f;

    [SerializeField]
    private GameObject player;

    protected void Start()
    {
        // spawnRate = initialSpawnRate;
        obstacleCount = 0;
        obstaclePosition = transform.position;
        obstacle = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, obstaclePosition, Quaternion.identity);
        if (obstacle != null)
        {
            obstaclePosition.z += obstacle.transform.localScale.z;
        }
    }

    protected void Update()
    {
        if (player.transform.position.z > obstaclePosition.z - spawnDistance && obstacleCount < maximumObstacleCount)//Time.time > lastSpawnTime + spawnRate && platformCount < maximumPlatformCount)
        {
            // Set position of platform
            obstacle = ObjectPooler.Instance.SpawnFromPool(Pool.PLATFORM, obstaclePosition, Quaternion.identity);
            obstaclePosition.z += obstacle.transform.localScale.z;

            obstacleComponent = obstacle.GetComponent<PlatformController>();
            obstacleComponent.OnObjectSpawn();
            obstacleComponent.onRemovePlatform += RemoveOne;

            obstacleCount++;
            // lastSpawnTime = Time.time;
        }
    }

    
    void RemoveOne()
    {
        obstacleCount--;
    }
}
