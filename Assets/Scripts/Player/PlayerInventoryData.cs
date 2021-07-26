using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerInventoryData : MonoBehaviour
{
    [SerializeField]
    private List<RecipeController> recipes;
    private List< Dictionary <string, int> > recipeList;

    public static Action UpdateRecipeUIEvent;

    [SerializeField]
    private GameObject recipeDisplay;
    private Animator recipeDisplayAnim;
    [SerializeField]
    private Text recipePoints;
    
    void Start()
    {
        recipeList = new List<Dictionary<string, int>>();
        foreach (RecipeController recipe in recipes) 
        {
            Dictionary <string, int> curDict = new Dictionary <string, int>();
            foreach (string ingredient in recipe.getListOfIngredients()) 
            {
                curDict.Add(ingredient, 0);
            }
            recipeList.Add(curDict);
        }
        recipeDisplayAnim = recipeDisplay.GetComponent<Animator>();
        recipePoints.text = "";

        if (UpdateRecipeUIEvent != null) 
        {
            UpdateRecipeUIEvent.Invoke();
        }
    }

    // Returns number of ingredients used up if dish is created
    public int AddIngredient(string ingredient)
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

        if (shouldCheckRecipes)
        {
            return checkRecipes();
        }

        if (UpdateRecipeUIEvent != null) 
        {
            UpdateRecipeUIEvent.Invoke();
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
    }

    private int checkRecipes()
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            Dictionary<string, int> recipeMap = recipeList[i];
            if (isRecipeCompleted(recipeMap))
            {
                removeCompletedRecipeIngredients(recipeMap);
                playCompletedRecipeAnimation(recipes[i].recipeName);
                int points = recipes[i].GetRecipePoints();
                displayRecipePoints(points);
                ScoreController.currentScore += points;

                return recipeMap.Count;
            }
        }

        return 0;
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

        if (UpdateRecipeUIEvent != null)
        {
            UpdateRecipeUIEvent.Invoke();
        }
    }

    private void playCompletedRecipeAnimation(string animationName)
    {
        switch(animationName) {
            case "burger": 
                recipeDisplayAnim.SetTrigger("CreateBurger");
                break;
            case "pizza":
                recipeDisplayAnim.SetTrigger("CreatePizza");
                break;
            case "sushi":
                recipeDisplayAnim.SetTrigger("CreateSushi");
                break;
            case "pasta":
                recipeDisplayAnim.SetTrigger("CreatePasta");
                break;
            case "salad":
                recipeDisplayAnim.SetTrigger("CreateSalad");
                break;
            case "applePie":
                recipeDisplayAnim.SetTrigger("CreatePie");
                break;
            case "taco":
                recipeDisplayAnim.SetTrigger("CreateTaco");
                break;
            default:
                break;
        }            
    }

    private void displayRecipePoints(int points)
    {
        recipePoints.text = "+ " + points.ToString();
        StartCoroutine(fadePointsText());
    }

    private IEnumerator fadePointsText()
    {
        recipePoints.color = new Color(recipePoints.color.r, recipePoints.color.g, recipePoints.color.b, 1);
        while (recipePoints.color.a > 0.0f)
        {            
            recipePoints.color = new Color(recipePoints.color.r, recipePoints.color.g, recipePoints.color.b, recipePoints.color.a - (Time.deltaTime / 2));
            yield return null;
        }
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

    // DON'T CHANGE THE ACTUAL LIST HERE!!! WE DECIDE ANIMATION AND POINTS BY recipes and recipeList being in same order
    public List <Dictionary <string, int> > GetRecipeProgress() {
        return recipeList;
    }

    public List<RecipeController> GetRecipes()
    {
        return recipes;
    }

    public int GetRecipeNumber()
    {
        return recipes.Count;
    }
}
