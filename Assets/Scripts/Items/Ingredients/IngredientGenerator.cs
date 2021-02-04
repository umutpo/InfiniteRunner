using UnityEngine;
using System.Collections;

public class IngredientGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private string[] selectIng = { Pool.INGREDIENT1, Pool.INGREDIENT2 };

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

    public string GetIngredient() {
        // TODO:
        // Return type of ingredient, use strings in Data.cs
        return selectIng[Random.Range(0, 2)];
    }
}
