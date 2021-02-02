using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryData : MonoBehaviour
{
    [SerializeField]
    private List<string> collectedIngredients;

    void Start()
    {
        collectedIngredients = new List<string>();
    }

    public void addIngredient(string ingredient)
    {
        collectedIngredients.Add(ingredient);
    }

    public void removeIngredient(string ingredient)
    {
        collectedIngredients.Remove(ingredient);
    }
}
