using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    public int freqIncrease = 1;

    private string[] selectIng = { Pool.INGREDIENT1, Pool.INGREDIENT2, 
    Pool.INGREDIENT3, Pool.INGREDIENT4, Pool.INGREDIENT5, Pool.INGREDIENT6};

    [SerializeField]
    private List<RecipeController> recipes;
    protected void Start()
    {
        // TODO:
        // Initialize rng variables
    }

    protected void Update()
    {
        // TODO:
        // Update rng variables based on player stats

    }

    public string GetIngredient() {
        // TODO:
        // Return type of ingredient, use strings in Data.cs
        PlayerController playerController = player.GetComponent<PlayerController>();
        Dictionary<string, int>  counts = playerController.getCollectedIngredientsCounts();
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
        foreach (string str in keyList)
        {
            if (counts[str] >= 1)
            {
                foreach(RecipeController recipe in recipes)
                {
                    List<string> ingredients = recipe.getListOfIngredients();
                    // if that recipe has the item already collected
                    if (ingredients.Contains(str))
                    {
                        foreach(string ingredient in ingredients)
                        {
                            // dont increase frequency for item already collected
                            if(ingredient != str)
                            {
                                int originalFreq = spawnProbabilities[ingredient];
                                spawnProbabilities[ingredient] = originalFreq + freqIncrease;
                            }
                        }
                    }
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
