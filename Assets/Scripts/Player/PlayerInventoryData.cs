using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventoryData : MonoBehaviour
{
    [SerializeField]
    private static Dictionary<Ingredient, int> ingredientList;
    public static Action<Ingredient> AddIngredientEvent;
    public static Action<Ingredient> RemoveIngredientEvent;
    // Start is called before the first frame update
    void Start()
    {
        ingredientList = new Dictionary<Ingredient, int>();
        foreach (Ingredient i in Ingredient.GetValues(typeof(Ingredient)))
        {
            ingredientList.Add(i, 0);
        }
    }

    public void AddIngredient(string ingredient)
    {
        Ingredient addedIngredient = FindInIngredientEnum(ingredient);
        ingredientList[addedIngredient]++;
        if (AddIngredientEvent != null)
            AddIngredientEvent(addedIngredient);
        else
            Debug.LogWarning("Update of ingredient nonexistent in UI was fired");
    }
    public void RemoveIngredient(string ingredient)
    {
        Ingredient removedIngredient = FindInIngredientEnum(ingredient);
        ingredientList[removedIngredient]--;
        if (RemoveIngredientEvent != null)
            RemoveIngredientEvent(removedIngredient);
        else
            Debug.LogWarning("Update of ingredient nonexistent in UI was fired");
    }
    private Ingredient FindInIngredientEnum(string ingredient)
    {
        ingredient = ingredient.Replace(" ", string.Empty);
        foreach (Ingredient i in Ingredient.GetValues(typeof(Ingredient)))
        {
            if (i.ToString() == ingredient)
                return i;
        }
        Debug.LogError("Ingredient string does not match any enum type. Returning Ingredient1 by default");
        return Ingredient.Ingredient1;
    }
}
