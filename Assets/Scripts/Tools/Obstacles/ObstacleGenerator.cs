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
    private float maxSpawnDist = 7f;
    private float minSpawnDist = 3f;
    private ObstacleController obstacleComponent;
    private Vector3 obstaclePosition;
    const float laneWidth = 3f;

    [SerializeField]
    private GameObject player;

    private string[] selectObs = {Pool.OBSTACLE1, Pool.OBSTACLE2};

    protected void Start()
    {
        obstacleCount = 0;
        obstaclePosition = player.transform.position;
    }

    protected void Update()
    {
        // TODO: change generation conditions
        obstaclePosition.x = 0f;
        if (obstacleCount < maximumObstacleCount) {            
            // Choose obstacle
            int obsIndex = Random.Range(0, selectObs.Length);
            int obsLane = Random.Range(-1, 2);
            // Set position of obstacle
            obstaclePosition.x += obsLane * laneWidth;
            obstaclePosition.z += Random.Range(minSpawnDist, maxSpawnDist);
            obstacle = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], obstaclePosition, Quaternion.identity);
            
            obstacleComponent = obstacle.GetComponent<ObstacleController>();
            obstacleComponent.OnObjectSpawn();
            obstacleComponent.onRemoveObstacle += RemoveOne;

            obstacleCount++;
        }
    }

    
    void RemoveOne()
    {
        obstacleCount--;
    }
}
