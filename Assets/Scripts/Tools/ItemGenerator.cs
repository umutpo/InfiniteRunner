using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private IngredientGenerator ingredient;
    private ObstacleGenerator obstacle;

    private int itemRowCount;
    [SerializeField]
    private int maximumItemRows = 8;    // max rows of items to generate ahead
    
    private GameObject item;
    [SerializeField]
    private int maxSpawnDist = 4;
    [SerializeField]
    private int minSpawnDist = 1;
    [SerializeField]
    private float spawnDistanceUnit = 5f;
    private ItemController itemComponent;
    private Vector3 itemPosition;
    const float laneWidth = 3f;

    [SerializeField]
    private GameObject player;

    private int rowRng;
    private string itemName;
    private int spawnLane;

    protected void Start()
    {
        ingredient = gameObject.GetComponent<IngredientGenerator>();
        obstacle = gameObject.GetComponent<ObstacleGenerator>();
        itemRowCount = 0;
        itemPosition = player.transform.position;
    }

    protected void Update()
    {
        // TODO: change generation conditions
        while (itemRowCount < maximumItemRows) {
            rowRng = Random.Range(1, 101);
            itemPosition.x = 0f;
            // Choose obstacle
            itemName = GetItemName();
            spawnLane = Random.Range(0, 3);
            // Set position of obstacle
            itemPosition.x += (spawnLane - 1) * laneWidth;
            // TODO: change to weighted, more closer distance?
            itemPosition.z += Random.Range(minSpawnDist, maxSpawnDist) * spawnDistanceUnit;
            item = ObjectPooler.Instance.SpawnFromPool(itemName, itemPosition, Quaternion.identity);
            
            itemComponent = item.GetComponent<ItemController>();
            itemComponent.OnObjectSpawn();
            itemComponent.onRemoveItem += RemoveOne;

            // 2 obstacles in same row
            if (rowRng > 70) {
                int laneRight = ((spawnLane + 1) % 3) - 1;
                itemName = GetItemName();
                itemPosition.x = laneRight * laneWidth;
                item = ObjectPooler.Instance.SpawnFromPool(itemName, itemPosition, Quaternion.identity);
                itemComponent = item.GetComponent<ItemController>();
                itemComponent.OnObjectSpawn();
                itemComponent.onRemoveItem += RemoveOne;
            }

            // 3 obstacles in same row
            if (rowRng > 90) {
                int laneLeft = (spawnLane + 2) % 3 - 1;
                itemName = GetItemName();
                itemPosition.x = laneLeft * laneWidth;
                item = ObjectPooler.Instance.SpawnFromPool(itemName, itemPosition, Quaternion.identity);
                itemComponent = item.GetComponent<ItemController>();
                itemComponent.OnObjectSpawn();
                itemComponent.onRemoveItem += RemoveOne;
            }
            itemRowCount++;
        }
    }

    string GetItemName() {
        // TODO:
        // Determine what type of item: Ingredient or obstacle
        int itemType = Random.Range(0, 2);
        // Return item name
        switch(itemType){
            case 0:
                return ingredient.GetIngredient();
            default:
                return obstacle.GetObstacle();
        }
    }

    
    void RemoveOne()
    {
        itemRowCount--;
    }
}
