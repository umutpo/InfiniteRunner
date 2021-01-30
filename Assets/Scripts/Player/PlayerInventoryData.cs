using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryData : MonoBehaviour
{
    [SerializeField]
    private List<string> collectedIngredients;
    [SerializeField]
    private float totalWeight = 0f;

    void Start()
    {
        collectedIngredients = new List<string>();
    }

    public void addIngredient(string ingredient, float weight = 1.0f)
    {
        collectedIngredients.Add(ingredient);
        totalWeight += weight;
    }

    public void removeIngredient(string ingredient)
    {
        collectedIngredients.Remove(ingredient);
    }
}
