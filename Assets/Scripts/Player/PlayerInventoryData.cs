using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventoryData : MonoBehaviour
{
    [SerializeField]
    private List<RecipeController> recipes;
    private List< Dictionary <string, int> > recipeList;

    public static Action<string, int, Sprite> AddIngredientEvent;
    public static Action<string, int> RemoveIngredientEvent;

    [SerializeField]
    private GameObject recipeDisplay;
    private Animator recipeDisplayAnim;

    void Start()
    {
        recipeList = new List<Dictionary<string, int>>();
        foreach (RecipeController recipe in recipes) 
        {
            Dictionary <string, int> curDict = new Dictionary <string, int>();
            foreach (IngredientController ingredient in recipe.ingredients) 
            {
                curDict.Add(ingredient.ingredient, 0);
            }
            recipeList.Add(curDict);
        }

        recipeDisplayAnim = recipeDisplay.GetComponent<Animator>();
    }

    // Returns number of ingredients used up if dish is created
    public int AddIngredient(string ingredient, Sprite inventoryImage)
    {
        bool shouldCheckRecipes = false;
        int obtainedIngredientCount = 1;
        foreach (Dictionary <string, int> recipeMap in recipeList) 
        {
            if (recipeMap.ContainsKey(ingredient)) 
            {
                recipeMap[ingredient]++;
                obtainedIngredientCount = recipeMap[ingredient];

                shouldCheckRecipes = shouldCheckRecipes || isRecipeCompleted(recipeMap);
            }
        }

        addIngredientUI(ingredient, obtainedIngredientCount, inventoryImage);

        if (shouldCheckRecipes)
        {
            return checkRecipes();
        }

        return 0;
    }
    
    public void RemoveIngredient(string ingredient)
    {
        int ingredientAmount = 0;
        foreach (Dictionary <string, int> recipeMap in recipeList) 
        {
            if (recipeMap.ContainsKey(ingredient) && recipeMap[ingredient] > 0) {
                recipeMap[ingredient]--;
                ingredientAmount = recipeMap[ingredient];
            }
        }

        removeIngredientUI(ingredient, ingredientAmount);
    }

    private int checkRecipes()
    {
        foreach (Dictionary <string, int> recipeMap in recipeList) 
        {
            if (isRecipeCompleted(recipeMap))
            {
                removeCompletedRecipeIngredients(recipeMap);
                // TODO: Set animation for correct recipe
                playCompletedRecipeAnimation("");
                ScoreController.currentScore += recipeMap.Count * 100;

                return recipeMap.Count;
            }
        }

        return 0;
    }

    private void addIngredientUI(string ingredient, int ingredientAmount, Sprite inventoryImage)
    {
        if (AddIngredientEvent != null && ingredient != null)
        {
            AddIngredientEvent(ingredient, ingredientAmount, inventoryImage);
        }
    }

    private void removeIngredientUI(string ingredient, int ingredientAmount)
    {
        if (RemoveIngredientEvent != null && ingredient != null)
        {
            RemoveIngredientEvent(ingredient, ingredientAmount);
        }
    }

    private bool isRecipeCompleted(Dictionary<string, int> recipeMap)
    {
        foreach (KeyValuePair<string, int> ingredientAmount in recipeMap)
        {
            if (ingredientAmount.Value < 1)
            {
                return false;
            }
        }

        return true;
    }

    private void removeCompletedRecipeIngredients(Dictionary<string, int> recipeMap)
    {
        List<string> ingredientsToBeRemoved = new List<string>();
        foreach (KeyValuePair<string, int> ingredientAmount in recipeMap)
        {
            ingredientsToBeRemoved.Add(ingredientAmount.Key);
        }
        foreach (string ingredient in ingredientsToBeRemoved)
        {
            RemoveIngredient(ingredient);
        }
    }

    private void playCompletedRecipeAnimation(string animationName)
    {
        if (recipeDisplayAnim)
        {
            recipeDisplayAnim.SetTrigger("CreateBurger");
        }
    }

    // recipe priority comparison rule; whichever needs the least number of ingredients to complete gets placed foremost
    private int closestToFinishFirst(Dictionary<string, int> recipe1, Dictionary<string, int> recipe2)
    {
        int missingIngredientCnt1 = 0;
        int missingIngredientCnt2 = 0;
        foreach (KeyValuePair<string, int> ingredientAmount in recipe1)
        {
            if (ingredientAmount.Value == 0)
                missingIngredientCnt1++;
        }
        foreach (KeyValuePair<string, int> ingredientAmount in recipe2)
        {
            if (ingredientAmount.Value == 0)
                missingIngredientCnt2++;
        }
        return missingIngredientCnt1 - missingIngredientCnt2;
    }

    public Dictionary <string, int> GetCollectedIngredientsCounts()
    {
        Dictionary <string, int> inventoryItems = new Dictionary<string, int>();
        foreach (Dictionary <string, int> recipeMap in recipeList) 
        {
            foreach (KeyValuePair<string, int> ingredientAmount in recipeMap)
            {
                if (!inventoryItems.ContainsKey(ingredientAmount.Key))
                {
                    inventoryItems.Add(ingredientAmount.Key, ingredientAmount.Value);
                }
            }
                
        }

        return inventoryItems;
    }

    // use this method to get the "pseudo priority queue" of recipes sorted by their ingredient completion rate
    public List <Dictionary <string, int> > GetRecipeProgress() {
        recipeList.Sort(closestToFinishFirst);
        return recipeList;
    }

    public List<RecipeController> GetRecipes()
    {
        return recipes;
    }
}
