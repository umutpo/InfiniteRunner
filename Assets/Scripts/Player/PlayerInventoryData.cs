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

    public static Action<Ingredient, int> AddIngredientEvent;
    public static Action<Ingredient, int> RemoveIngredientEvent;

    void Start()
    {
        collectedIngredients = new List<string>();
        collectedIngredientsCounts = new Dictionary<string, int>();
    }

    // Returns number of ingredients used up if dish is created
    public int AddIngredient(string ingredient)
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

        addIngredientUI(ingredient);

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

    private Ingredient findInIngredientEnum(string ingredient)
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

    private void addIngredientUI(string ingredient)
    {
        if (AddIngredientEvent != null)
        {
            Ingredient addedIngredient = findInIngredientEnum(ingredient);
            AddIngredientEvent(addedIngredient, collectedIngredientsCounts[ingredient]);
        }
    }

    private void removeIngredientUI(string ingredient)
    {
        if (RemoveIngredientEvent != null)
        {
            Ingredient removedIngredient = findInIngredientEnum(ingredient);
            RemoveIngredientEvent(removedIngredient, collectedIngredientsCounts[ingredient]);
        }
    }
}
