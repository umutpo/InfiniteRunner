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

    public static Action<string, int, Sprite> AddIngredientEvent;
    public static Action<string, int> RemoveIngredientEvent;

    void Start()
    {
        collectedIngredients = new List<string>();
        collectedIngredientsCounts = new Dictionary<string, int>();
    }

    // Returns number of ingredients used up if dish is created
    public int AddIngredient(string ingredient, Sprite inventoryImage)
    {
        bool shouldCheckRecipes = false;

        if (collectedIngredients.Contains(ingredient))
        {
            collectedIngredientsCounts[ingredient]++;
        }
        else
        {
            collectedIngredients.Add(ingredient);
            collectedIngredientsCounts.Add(ingredient, 1);
            shouldCheckRecipes = true;
        }

        addIngredientUI(ingredient, inventoryImage);

        if (shouldCheckRecipes)
        {
            return checkRecipes();
        }

        return 0;
    }
    
    public void RemoveIngredient(string ingredient)
    {
        collectedIngredientsCounts[ingredient]--;

        removeIngredientUI(ingredient);

        if (collectedIngredientsCounts[ingredient] <= 0)
        {
            collectedIngredients.Remove(ingredient);
            collectedIngredientsCounts.Remove(ingredient);
        }
    }

    private int checkRecipes()
    {
        foreach (RecipeController recipe in recipes) 
        {
            bool canCreateRecipe = recipe.ingredients.TrueForAll(doesContainIngredient);
            if (canCreateRecipe)
            {
                foreach (IngredientController ingredientController in recipe.ingredients)
                {
                    RemoveIngredient(ingredientController.ingredient);
                }
                ScoreController.currentScore += recipe.ingredients.Count * 100;
                return recipe.ingredients.Count;
            }
        }
        return 0;
    }

    private bool doesContainIngredient(IngredientController ingredientController)
    {
        return collectedIngredients.Contains(ingredientController.ingredient);
    }


    private void addIngredientUI(string ingredient, Sprite inventoryImage)
    {
        if (AddIngredientEvent != null && ingredient != null)
        {
            AddIngredientEvent(ingredient, collectedIngredientsCounts[ingredient], inventoryImage);
        }
    }

    private void removeIngredientUI(string ingredient)
    {
        if (RemoveIngredientEvent != null && ingredient != null)
        {
            RemoveIngredientEvent(ingredient, collectedIngredientsCounts[ingredient]);
        }
    }

    public Dictionary<string, int> getCollectedIngredientsCounts()
    {
        return collectedIngredientsCounts;
    }
}
