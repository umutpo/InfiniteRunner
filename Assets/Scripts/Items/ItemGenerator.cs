using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private IngredientGenerator ingredient;
    private ObstacleGenerator obstacle;
    [SerializeField]  private KitchenGenerator kitchen;
    const int INFINITY = 1000000000;

    //private GameObject item;
    //private ItemController itemComponent;
    //private Vector3 itemPosition;
    //private int rowRng;
    //private string itemName;
    //private int spawnLane;
    [SerializeField]
    private GameObject player;

    [Header("Constant Characteristics")]
    [SerializeField]
    private float spawnDistanceUnit = 5f;
    [SerializeField]
    private float laneWidth = 3f;
    // Number of rows ahead of player the generator will spawn items
    [SerializeField]
    private float generateAheadDistance = 8;
    // Number of blank rows at beginning of game
    [SerializeField]
    private float startingItemRow = 15;

    [Header("Starting Characteristics")]
    [Range(0, 100), SerializeField] private int ingredientVsObstacleBias;
    [SerializeField]  private int itemDensity;
    [SerializeField]  private int singleItemRowWeight;
    [SerializeField]  private int doubleItemRowWeight;
    [SerializeField]  private int tripleItemRowWeight;

    [Header("Characteristic Progression")]
    [SerializeField] private float deltaDurationInSeconds;
    [Range(-20, 20), SerializeField] private int deltaIngredientVsObstacleBias;
    [SerializeField]  private int deltaItemDensity;
    [SerializeField]  private int deltaSingleItemRowWeight;
    [SerializeField]  private int deltaDoubleItemRowWeight;
    [SerializeField]  private int deltaTripleItemRowWeight;

    [Header("Characteristic Cap")]
    [Range(0, 100), SerializeField] private int capIngredientVsObstacleBias;
    [SerializeField]  private int capItemDensity;
    [SerializeField]  private int capSingleItemRowWeight;
    [SerializeField]  private int capDoubleItemRowWeight;
    [SerializeField]  private int capTripleItemRowWeight;

    private float lastDeltaTime = 0;
    private float lastGenerateSpot;

    protected void Start()
    {
        ingredient = gameObject.GetComponent<IngredientGenerator>();
        obstacle = gameObject.GetComponent<ObstacleGenerator>();
        
        lastGenerateSpot = startingItemRow * spawnDistanceUnit;
        lastDeltaTime = Time.time;
    }

    protected void FixedUpdate()
    {

        if (Time.time - lastDeltaTime >= deltaDurationInSeconds)
        {
            AdjustCharacteristics();
            lastDeltaTime = Time.time;
        }

        if (player.transform.position.z + spawnDistanceUnit * generateAheadDistance >= lastGenerateSpot)
        {
            if (Random.Range(0, 100) < itemDensity)
            {
                int rowRng = Random.Range(0, singleItemRowWeight + doubleItemRowWeight + tripleItemRowWeight);
                int startLane = Random.Range(0, 3);
                int itemCntInRow = 1 + (rowRng >= singleItemRowWeight ? 1 : 0) + (rowRng >= singleItemRowWeight + doubleItemRowWeight ? 1 : 0);
                
                int itemsPlaced = 0;
                int openLanes = 3;
                if (kitchen.GetObstacleEnd(0) > lastGenerateSpot)
                {
                    openLanes--;
                }
                if (kitchen.GetObstacleEnd(1) > lastGenerateSpot)
                {
                    openLanes--;
                }

                for (int i = startLane; i < startLane + itemCntInRow; i++)
                {
                    int lane = i % 3;
                    if (lane == 0 && kitchen.GetObstacleEnd(0) > lastGenerateSpot) {
                        continue;
                    }
                    if (lane == 2 && kitchen.GetObstacleEnd(1) > lastGenerateSpot) {
                        continue;
                    }
                    Vector3 itemPosition = new Vector3((lane - 1) * laneWidth, 1, lastGenerateSpot);
                    string itemName = GetItemName();
                    GameObject item = ObjectPooler.Instance.SpawnFromPool(itemName, itemPosition, Quaternion.identity);
                    if (item == null) continue;
                    ItemController itemComponent = item.GetComponent<ItemController>();
                    itemComponent.OnObjectSpawn();
                    itemsPlaced++;
                }
                if (itemsPlaced == openLanes)
                    lastGenerateSpot += spawnDistanceUnit; // give player time after a row full of obstacles and items by making next row blank
            }

            lastGenerateSpot += spawnDistanceUnit;
        }
    }

    void AdjustCharacteristics()
    {
        singleItemRowWeight = ClampedAddition(singleItemRowWeight, deltaSingleItemRowWeight, capSingleItemRowWeight);
        doubleItemRowWeight = ClampedAddition(doubleItemRowWeight, deltaDoubleItemRowWeight, capDoubleItemRowWeight);
        tripleItemRowWeight = ClampedAddition(tripleItemRowWeight, deltaTripleItemRowWeight, capTripleItemRowWeight);
        ingredientVsObstacleBias = ClampedAddition(ingredientVsObstacleBias, deltaIngredientVsObstacleBias, capIngredientVsObstacleBias);
        itemDensity = ClampedAddition(itemDensity, deltaItemDensity, capItemDensity);
    }

    int ClampedAddition(int currentValue, int deltaValue, int capValue)
    {
        if (currentValue == capValue || (currentValue + deltaValue >= capValue && currentValue < capValue) || (currentValue + deltaValue <= capValue && currentValue > capValue))
            return capValue;
        else return currentValue + deltaValue;
    }

    string GetItemName() {
        // Determine what type of item: Ingredient or obstacle
        int itemTypeRng = Random.Range(0, 100);
        // Return item name
        if (itemTypeRng < ingredientVsObstacleBias) 
            return ingredient.GetIngredient();
        else
            return obstacle.GetObstacle();
        
    }

}
