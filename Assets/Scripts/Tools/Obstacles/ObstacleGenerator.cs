using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private int obstacleRowCount;
    [SerializeField]
    private int maximumObstacleRows = 8;
    
    private GameObject obstacle;
    [SerializeField]
    private int maxSpawnDist = 4;
    private int minSpawnDist = 1;
    private float spawnDistanceUnit = 5f;
    private ObstacleController obstacleComponent;
    private Vector3 obstaclePosition;
    const float laneWidth = 3f;

    [SerializeField]
    private GameObject player;

    private string[] selectObs = {Pool.OBSTACLE1, Pool.OBSTACLE2};

    protected void Start()
    {
        obstacleRowCount = 0;
        obstaclePosition = player.transform.position;
    }

    protected void Update()
    {
        // TODO: change generation conditions
        while (obstacleRowCount < maximumObstacleRows) {
            int rowRng = Random.Range(1, 101);
            obstaclePosition.x = 0f;
            // Choose obstacle
            int obsIndex = Random.Range(0, selectObs.Length);
            int obsLane = Random.Range(0, 3);
            // Set position of obstacle
            obstaclePosition.x += (obsLane - 1) * laneWidth;
            // TODO: change to weighted, more closer distance?
            obstaclePosition.z += Random.Range(minSpawnDist, maxSpawnDist) * spawnDistanceUnit;
            obstacle = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], obstaclePosition, Quaternion.identity);
            
            obstacleComponent = obstacle.GetComponent<ObstacleController>();
            obstacleComponent.OnObjectSpawn();
            obstacleComponent.onRemoveObstacle += RemoveOne;

            // 2 obstacles in same row
            if (rowRng > 70) {
                int laneRight = ((obsLane + 1) % 3) - 1;
                obsIndex = Random.Range(0, selectObs.Length);
                obstaclePosition.x = laneRight * laneWidth;
                obstacle = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], obstaclePosition, Quaternion.identity);
                obstacleComponent = obstacle.GetComponent<ObstacleController>();
                obstacleComponent.OnObjectSpawn();
                obstacleComponent.onRemoveObstacle += RemoveOne;
            }

            // 3 obstacles in same row
            if (rowRng > 90) {
                int laneLeft = (obsLane + 2) % 3 - 1;
                obsIndex = Random.Range(0, selectObs.Length);
                obstaclePosition.x = laneLeft * laneWidth;
                obstacle = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], obstaclePosition, Quaternion.identity);
                obstacleComponent = obstacle.GetComponent<ObstacleController>();
                obstacleComponent.OnObjectSpawn();
                obstacleComponent.onRemoveObstacle += RemoveOne;
            }
            obstacleRowCount++;
        }
    }

    
    void RemoveOne()
    {
        obstacleRowCount--;
    }
}
