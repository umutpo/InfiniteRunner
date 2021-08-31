using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    private enum KITCHEN_TYPES { LEFT, RIGHT, OBSTACLE_LEFT, OBSTACLE_RIGHT };

    private int kitchenCount;
    [SerializeField]
    private int maximumKitchenCount = 20;
    
    private GameObject kitchen;
    [SerializeField]
    private float spawnDistance = 200;
    private PlatformController kitchenComponent;
    private Vector3 leftPosition;
    private Vector3 rightPosition;
    private Vector3 laneOffset;
    // TODO: change according to model
    private float kitchenLength = 15f;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private bool isTutorial = false;

    private string[] LEFT_KITCHENS = { Pool.KITCHEN1, Pool.KITCHEN2, Pool.KITCHEN3, Pool.KITCHEN4, Pool.KITCHEN5 };
    private string[] RIGHT_KITCHENS = {  Pool.KITCHEN6, Pool.KITCHEN7, Pool.KITCHEN8, Pool.KITCHEN9, Pool.KITCHEN10 };
    private string[] OBSTACLE_LEFT_KITCHENS = { Pool.OBSTACLEKIT1, Pool.OBSTACLEKIT2, Pool.OBSTACLEKIT3, Pool.OBSTACLEKIT4, Pool.OBSTACLEKIT5, Pool.OBSTACLEKIT11, Pool.OBSTACLEKIT12, Pool.OBSTACLEKIT13, Pool.OBSTACLEKIT17 };
    private string[] OBSTACLE_RIGHT_KITCHENS = { Pool.OBSTACLEKIT6, Pool.OBSTACLEKIT7, Pool.OBSTACLEKIT8, Pool.OBSTACLEKIT9, Pool.OBSTACLEKIT10, Pool.OBSTACLEKIT14, Pool.OBSTACLEKIT15, Pool.OBSTACLEKIT16, Pool.OBSTACLEKIT18};

    private Vector3[] pos = new Vector3[2];
    private float[] obstacleEnds = {-1, -1};    // {left lane obstacle end, right lane obstacle end}


    protected void Start()
    {
        kitchenCount = 0;
        leftPosition = transform.position + new Vector3(-7.5f, 0, 0);
        rightPosition = transform.position + new Vector3(7.5f, 0, 0);
        pos[0] = leftPosition;
        pos[1] = rightPosition;
        kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(KITCHEN_TYPES.LEFT), leftPosition, Quaternion.identity);
        if (kitchen != null)
        {
            leftPosition.z += kitchen.transform.localScale.z * kitchenLength;
        }
        kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(KITCHEN_TYPES.RIGHT), rightPosition, Quaternion.identity);
        if (kitchen != null)
        {
            rightPosition.z += kitchen.transform.localScale.z * kitchenLength;
        }
    }

    protected void Update()
    {
        AddKitchen(0);
        AddKitchen(1);
    }

    private void AddKitchen(int posIndex) {
        float spawnRange = pos[posIndex].z - spawnDistance;        
        if (player.transform.position.z > spawnRange && kitchenCount < maximumKitchenCount)
        {
            KITCHEN_TYPES currentKitchenType = KITCHEN_TYPES.LEFT;

            // Determine if kitchen is lane obstacle
            if (!isTutorial && OBSTACLE_LEFT_KITCHENS.Length > 0 && OBSTACLE_RIGHT_KITCHENS.Length > 0 && Random.Range(0, 100) < 10) {
                currentKitchenType = posIndex == 0 ? KITCHEN_TYPES.OBSTACLE_LEFT : KITCHEN_TYPES.OBSTACLE_RIGHT;
            } else {
                currentKitchenType = posIndex == 0 ? KITCHEN_TYPES.LEFT : KITCHEN_TYPES.RIGHT;
            }

            // Set position of platform
            kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(currentKitchenType), pos[posIndex], Quaternion.identity);
            // Check if platform spawned
            if (kitchen == null) return;   // Should never occur if pool size is larger than max
            pos[posIndex].z += kitchen.transform.localScale.z * kitchenLength;

            kitchenComponent = kitchen.GetComponent<PlatformController>();
            kitchenComponent.OnObjectSpawn();
            kitchenComponent.onRemoveItem += RemoveOne;

            kitchenCount++;
        }
    }

    private string GetRandomKitchen(KITCHEN_TYPES kitchen_type) {
        string kitchen = "";
        int kit = 0;
        switch(kitchen_type)
        {
            case KITCHEN_TYPES.LEFT:
                kit = Random.Range(0, LEFT_KITCHENS.Length);
                kitchen = LEFT_KITCHENS[kit];
                break;
            case KITCHEN_TYPES.RIGHT:
                kit = Random.Range(0, RIGHT_KITCHENS.Length);
                kitchen = RIGHT_KITCHENS[kit];
                break;
            case KITCHEN_TYPES.OBSTACLE_LEFT:
                kit = Random.Range(0, OBSTACLE_LEFT_KITCHENS.Length);
                kitchen = OBSTACLE_LEFT_KITCHENS[kit];
                break;
            case KITCHEN_TYPES.OBSTACLE_RIGHT:
                kit = Random.Range(0, OBSTACLE_RIGHT_KITCHENS.Length);
                kitchen = OBSTACLE_RIGHT_KITCHENS[kit];
                break;
            default:
                break;
        }

        return kitchen;
    }

    // lane is 0 for left, 1 for right
    public float GetObstacleEnd(int lane) {
        return obstacleEnds[lane];
    }

    
    void RemoveOne()
    {
        kitchenCount--;
    }
}
