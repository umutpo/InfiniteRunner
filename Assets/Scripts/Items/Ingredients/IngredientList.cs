using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientList : MonoBehaviour
{
    private static Dictionary<Ingredient, int> ingredientList;
    public delegate void SetIngredientAction(Ingredient ingredient);
    public static event SetIngredientAction AddIngredientEvent;
    public static event SetIngredientAction RemoveIngredientEvent;
    // Start is called before the first frame update
    void Start()
    {
        ingredientList = new Dictionary<Ingredient, int>();
        foreach (Ingredient i in Ingredient.GetValues(typeof(Ingredient)))
        {
            ingredientList.Add(i, 0);
        }
    }

    public static void AddIngredient(Ingredient addedIngredient)
    {
        ingredientList[addedIngredient]++;
        if (AddIngredientEvent != null)
            AddIngredientEvent(addedIngredient);
        else
            Debug.LogWarning("Update of ingredient nonexistent in UI was fired");
    }
    public static void RemoveIngredient(Ingredient removedIngredient)
    {
        ingredientList[removedIngredient]--;
        if (RemoveIngredientEvent != null)
            RemoveIngredientEvent(removedIngredient);
        else
            Debug.LogWarning("Update of ingredient nonexistent in UI was fired");
    }
}
