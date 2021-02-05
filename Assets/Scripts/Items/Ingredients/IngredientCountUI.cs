using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing an ingredient on the UI with a text component whose value is the current count of the ingredient in inventory
public class IngredientCountUI : MonoBehaviour
{
    [SerializeField] private Ingredient ingredientType;

    private int ingredientCount = 0;
    
    void OnEnable()
    {
        PlayerInventoryData.AddIngredientEvent += AddIngredient;
        PlayerInventoryData.RemoveIngredientEvent += RemoveIngredient;
    }
    void OnDisable()
    {
        PlayerInventoryData.AddIngredientEvent -= AddIngredient;
        PlayerInventoryData.RemoveIngredientEvent -= RemoveIngredient;
    }
    void AddIngredient(Ingredient ing)
    {
        if (ingredientType == ing)
        {
            ingredientCount++;
            UpdateCount();
        }
    }
    void RemoveIngredient(Ingredient ing)
    {
        if (ingredientType == ing)
        {
            ingredientCount--;
            UpdateCount();
        }
    }
    void UpdateCount()
    {
        gameObject.GetComponent<Text>().text = ingredientCount.ToString();
    }
}
