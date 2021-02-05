using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing an ingredient on the UI with a text component whose value is the current count of the ingredient in inventory
public class IngredientCountUI : MonoBehaviour
{
    [SerializeField] private Ingredient ingredientType;
    
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

    void AddIngredient(Ingredient ing, int count)
    {
        if (ingredientType == ing)
        {
            UpdateCount(count);
        }
    }

    void RemoveIngredient(Ingredient ing, int count)
    {
        if (ingredientType == ing)
        {
            UpdateCount(count);
        }
    }

    void UpdateCount(int count)
    {
        gameObject.GetComponent<Text>().text = count.ToString();
    }
}
