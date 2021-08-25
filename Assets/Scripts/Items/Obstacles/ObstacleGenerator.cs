using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private string[] OBSTACLES = { Pool.OBSTACLE1, Pool.OBSTACLE2, Pool.OBSTACLE3 };

    protected void Start()
    {
    }

    protected void Update()
    {
    }

    public string GetObstacle() {
        return OBSTACLES[Random.Range(0, OBSTACLES.Length)];
    }
}
