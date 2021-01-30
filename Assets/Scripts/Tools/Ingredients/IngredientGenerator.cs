using UnityEngine;
using System.Collections;

public class IngredientGenerator : MonoBehaviour
{
    private int ingredientRowCount;
    [SerializeField]
    private int maximumIngredientRows = 8;

    private GameObject ingredient;
    [SerializeField]
    private int maxSpawnDist = 4;
    private int minSpawnDist = 1;
    private float spawnDistanceUnit = 5f;
    private IngredientController ingredientComponent;
    private Vector3 ingredientPosition;
    const float laneWidth = 3f;

    [SerializeField]
    private GameObject player;

    private string[] selectObs = { Pool.INGREDIENT1, Pool.INGREDIENT2 };

    protected void Start()
    {
        ingredientRowCount = 0;
        ingredientPosition = player.transform.position;
    }

    protected void Update()
    {
        // TODO: change generation conditions
        while (ingredientRowCount < maximumIngredientRows)
        {
            int rowRng = Random.Range(1, 101);
            ingredientPosition.x = 0f;
            // Choose ingredient
            int obsIndex = Random.Range(0, selectObs.Length);
            int obsLane = Random.Range(0, 3);
            // Set position of ingredient
            ingredientPosition.x += (obsLane - 1) * laneWidth;
            // TODO: change to weighted, more closer distance?
            ingredientPosition.z += Random.Range(minSpawnDist, maxSpawnDist) * spawnDistanceUnit;
            ingredient = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], ingredientPosition, Quaternion.identity);

            ingredientComponent = ingredient.GetComponent<IngredientController>();
            ingredientComponent.OnObjectSpawn();
            ingredientComponent.onRemoveIngredient += RemoveOne;

            // 2 ingredients in same row
            if (rowRng > 70)
            {
                int laneRight = ((obsLane + 1) % 3) - 1;
                obsIndex = Random.Range(0, selectObs.Length);
                ingredientPosition.x = laneRight * laneWidth;
                ingredient = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], ingredientPosition, Quaternion.identity);
                ingredientComponent = ingredient.GetComponent<IngredientController>();
                ingredientComponent.OnObjectSpawn();
                ingredientComponent.onRemoveIngredient += RemoveOne;
            }

            // 3 ingredients in same row
            if (rowRng > 90)
            {
                int laneLeft = (obsLane + 2) % 3 - 1;
                obsIndex = Random.Range(0, selectObs.Length);
                ingredientPosition.x = laneLeft * laneWidth;
                ingredient = ObjectPooler.Instance.SpawnFromPool(selectObs[obsIndex], ingredientPosition, Quaternion.identity);
                ingredientComponent = ingredient.GetComponent<IngredientController>();
                ingredientComponent.OnObjectSpawn();
                ingredientComponent.onRemoveIngredient += RemoveOne;
            }
            ingredientRowCount++;
        }
    }


    void RemoveOne()
    {
        ingredientRowCount--;
    }
}
