using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryData : MonoBehaviour
{
    //TODO remove after official ingredients get added
    static string testIngredient1 = "test1";
    static string testIngredient2 = "test2";

    [SerializeField] private List<string> collectedIngredients;
    // each item is <ingredient Name, weight>
    public IDictionary<string, int> ingredientList = new Dictionary<string, int>();

    //key is ingredient name, int is the number collected
    public IDictionary<string, int> collectedItems = new Dictionary<string, int>();

    // key is recipe Hashset, value is points given by score
    private IDictionary<HashSet<string>, int> scoreSet = new Dictionary<HashSet<string>, int>();

    //all the recipes here
    //TODO Remove after official recipe gets added
    private HashSet<string> testRecipe = new HashSet<string> {testIngredient1, testIngredient2};

    void Start()
    {
        collectedIngredients = new List<string>();
        //initialize recipes and their scores
        initializeIngredients();
        initializeScoreSet();
    }

    // Update is called once per frame
    void Update()
    {
        // should maybe change the following into a switch case statement
        #region check which dish is met, huge if statement
        if (testRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(testRecipe);
        }

        #endregion
    }

    public void addIngredient(string ingredient)
    {
        collectedIngredients.Add(ingredient);
    }

    public void removeIngredient(string ingredient)
    {
        collectedIngredients.Remove(ingredient);
    }

    void initializeIngredients()
    {
        ingredientList.Add(testIngredient1, 200);
        ingredientList.Add(testIngredient2, 200);
    }
    void initializeScoreSet()
    {
        scoreSet.Add(testRecipe, RecipeScore(testRecipe));
    }
    // return the score of the dish provided the recipe
    int RecipeScore(HashSet<string> recipe)
    {
        int score = 0;
        foreach (string str in recipe)
        {
            int ingScore;
            ingredientList.TryGetValue(str, out ingScore);
            score += ingScore;
        }
        return score;
    }

    // remove ingredients from collected list after making a dish
    void UseIngredients(HashSet<string> recipe)
    {
        int num;
        foreach (string str in recipe)
        {
            collectedItems.TryGetValue(str, out num);
            if (num > 1)
            {
                collectedItems.Remove(str);
                collectedItems.Add(str, num - 1);
            }
            else
            {
                collectedItems.Remove(str);
            }
        }
    }

    // making a dish and getting the points
    void MakeDish(HashSet<string> recipe)
    {
        UseIngredients(recipe);
        int earnedPoints;
        scoreSet.TryGetValue(recipe, out earnedPoints);
        ScoreController.currentScore += earnedPoints;
        speedIncreaseAfterDish(earnedPoints);
    }

    void speedIncreaseAfterDish(int points)
    {
        PlayerController playerController = GetComponent<PlayerController>();
        float speedIncrease = (points / 100);
        playerController.SpeedUp(speedIncrease);
    }
}
