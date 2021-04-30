using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private string[] selectObs = { Pool.OBSTACLE1, Pool.OBSTACLE2, Pool.OBSTACLE3};

    protected void Start()
    {
        // TODO:
        // Initialize rng variables
    }

    protected void Update()
    {
        // TODO:
        // Update rng variables based on player stats

    }

    public string GetObstacle() {
        // TODO:
        // Return type of ingredient, use strings in Data.cs
        return selectObs[Random.Range(0, selectObs.Length)];
    }
}
