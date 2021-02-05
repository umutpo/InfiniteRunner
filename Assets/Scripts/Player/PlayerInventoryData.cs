using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventoryData : MonoBehaviour
{
    [SerializeField]
    private List<string> collectedIngredients;
    private Dictionary<string, int> collectedIngredientsCounts;

    [SerializeField]
    private List<RecipeController> recipes;

    void Start()
    {
        collectedIngredients = new List<string>();
        collectedIngredientsCounts = new Dictionary<string, int>();
    }

    public void AddIngredient(string ingredient)
    {
        if (collectedIngredients.Contains(ingredient))
        {
            collectedIngredientsCounts[ingredient]++;
        }
        else
        {
            collectedIngredients.Add(ingredient);
            collectedIngredientsCounts.Add(ingredient, 1);
            checkRecipes();
        }
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
        collectedIngredientsCounts[ingredient]--;
        if (collectedIngredientsCounts[ingredient] <= 0)
        {
            collectedIngredients.Remove(ingredient);
            collectedIngredientsCounts.Remove(ingredient);
        }
    }

    private void checkRecipes()
    {
        foreach (RecipeController recipe in recipes) 
        {
            bool canCreateRecipe = recipe.ingredients.TrueForAll(doesContainIngredient);
            if (canCreateRecipe)
            {
                foreach (IngredientController ingredientController in recipe.ingredients)
                {
                    removeIngredient(ingredientController.ingredient);
                }
                ScoreController.currentScore += recipe.ingredients.Count * 100;
            }
        }
    }

    private bool doesContainIngredient(IngredientController ingredientController)
    {
        return collectedIngredients.Contains(ingredientController.ingredient);
    }
}
