using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    public int freqIncrease = 1;

    private string[] selectIng = { 
        Pool.FLOUR, Pool.TOMATO, Pool.CHEESE, Pool.PASTA,
        Pool.MEATBALL, Pool.BUN, Pool.PATTY, Pool.LETTUCE, 
        Pool.RICE, Pool.FISH, Pool.SEAWEED, Pool.TACO_SHELL, 
        Pool.APPLE, Pool.SUGAR
    };

    private PlayerController playerController;

    protected void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    protected void Update()
    {
    }

    public string GetIngredient() {
        // TODO:
        // Return type of ingredient, use strings in Data.cs
        Dictionary<string, int> counts = playerController.GetCollectedIngredientsCounts();
        Dictionary<string, int> spawnProbabilities = initializeSpawnProbabilities();
        updateSpawnProbabilities(counts, spawnProbabilities);
        List<string> ingredientPool = generatePool(spawnProbabilities);
        int index = Random.Range(0, ingredientPool.Count);
        return ingredientPool[index];
    }

    private Dictionary<string, int> initializeSpawnProbabilities()
    {
        Dictionary<string, int> spawnProbabilities = new Dictionary<string, int>();
        for (int i = 0; i < selectIng.Length; i++)
        {
            spawnProbabilities.Add(selectIng[i], 1);
        }
        return spawnProbabilities;
    }

    // update probablity for each ingredient
    // adds frequency to items left to complete a recipe
    private void updateSpawnProbabilities(Dictionary<string, int> counts, Dictionary<string, int> spawnProbabilities)
    {
        List<string> keyList = new List<string>(counts.Keys);
        List<RecipeController> recipes = playerController.GetRecipes();
        // iterate over each recipe
        foreach (RecipeController recipe in recipes)
        {
            List<string> ingredients = recipe.getListOfIngredients();
            List<string> ingredientsLeftToComplete = new List<string>();
            bool halfFinished = false;
            // iterate over all the ingredients needed for this recipe
            // check to see if anything for this recipe is collected
            foreach (string ingredient in ingredients)
            {
                if (counts[ingredient] == 0)
                {
                    ingredientsLeftToComplete.Add(ingredient);
                }
            }
            // 
            if(ingredientsLeftToComplete.Count != ingredients.Count
            && ingredientsLeftToComplete.Count != 0)
            {
                halfFinished = true;
            }
            // increase freqency for ingredientsLeftToComplete if recipe is halffinished
            // do not change anything is the recipe has nothing collected yet or can be finished as  a dish
            if(halfFinished)
            {
                foreach (string ingredientToIncrease in ingredientsLeftToComplete)
                {
                    int originalFreq = spawnProbabilities[ingredientToIncrease];
                    spawnProbabilities[ingredientToIncrease] = originalFreq + freqIncrease;
                }
            }
        }
    }
    //generate an ingredient pool based on given spawn frequency
    private List<string> generatePool(Dictionary<string, int> spawnProbabilities)
    {
        List<string> ingredientPool = new List<string>();
        List<string> keys = new List<string>(spawnProbabilities.Keys);
        foreach (string key in keys)
        {
            for(int i = 0; i < spawnProbabilities[key]; i++)
            {
                ingredientPool.Add(key);
            }
        }
        return ingredientPool;
    }
}
