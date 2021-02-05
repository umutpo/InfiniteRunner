using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollection : MonoBehaviour
{

    public GameObject Ball;

    #region ingredient variables
    // list all ingredient variables
    static string bun = "bun";
    static string sausage = "sausage";
    static string ketchup = "ketchup";
    static string noodle = "noodle";
    static string cheese = "cheese";
    static string patty = "patty";
    static string tomato = "tomato";
    static string onion = "onion";
    static string lettuce = "lettuce";
    static string spinach = "spinach";
    static string egg = "egg";
    static string olive_oil = "olive oil";
    static string baguette = "baguette";
    static string rice = "rice";
    static string shrimp = "shrimp";
    static string eggplant = "eggplant";
    static string pumpkin = "pumpkin";
    static string steak = "steak";
    static string potato = "potato";
    static string drumstick = "drumstick";
    static string wing = "wing";
    static string mayo = "mayo";
    static string flour = "flour";
    static string carrot = "carrot";
    static string penne = "penne";
    static string pesto = "pesto";
    static string chicken = "chicken";
    //TODO
    static string testIngredient = "test";

    #endregion

    #region recipe HashSets
    private HashSet<string> hamburgerRecipe = new HashSet<string> { bun, patty, ketchup, lettuce, tomato, onion, cheese };
    private HashSet<string> hotdogRecipe = new HashSet<string> { bun, sausage, ketchup };
    private HashSet<string> chefSaladRecipe = new HashSet<string> { tomato, lettuce, spinach, onion, olive_oil, baguette };
    private HashSet<string> spaghettiCarbonaraRecipe = new HashSet<string> { noodle, sausage, cheese };
    private HashSet<string> ratatouilleRecipe = new HashSet<string> { tomato, eggplant, pumpkin, olive_oil, onion };
    private HashSet<string> tenderloinSteakRecipe = new HashSet<string> { steak, potato, onion };
    private HashSet<string> kidsMealRecipe = new HashSet<string> { olive_oil, chicken, potato, ketchup };
    private HashSet<string> sandwichRecipe = new HashSet<string> { baguette, egg, cheese, tomato, mayo, chicken };
    private HashSet<string> tempuraRecipe = new HashSet<string> { shrimp, egg, flour };
    private HashSet<string> pumpkinPieRecipe = new HashSet<string> { pumpkin, flour, egg };
    //TODO
    private HashSet<string> testRecipe = new HashSet<string> {testIngredient};

    #endregion

    // each item is <ingredient Name, weight>
    public IDictionary<string, int> ingredientList = new Dictionary<string, int>();

    //key is ingredient name, int is the number collected
    public IDictionary<string, int> collectedItems = new Dictionary<string, int>();

    // key is recipe Hashset, value is points given by score
    private IDictionary<HashSet<string>, int> scoreSet = new Dictionary<HashSet<string>, int>();

    // Use this for initialization
    void Start()
    {

        #region add ingredients and scores to ingredientList
        ingredientList.Add(tomato, 50);
        ingredientList.Add(lettuce, 50);
        ingredientList.Add(carrot, 50);
        ingredientList.Add(onion, 50);
        ingredientList.Add(potato, 50);
        ingredientList.Add(spinach, 50);
        ingredientList.Add(baguette, 50);
        ingredientList.Add(ketchup, 50);
        ingredientList.Add(olive_oil, 50);
        ingredientList.Add(mayo, 50);
        ingredientList.Add(eggplant, 100);
        ingredientList.Add(pumpkin, 100);
        ingredientList.Add(sausage, 100);
        ingredientList.Add(egg, 100);
        ingredientList.Add(noodle, 100);
        ingredientList.Add(penne, 100);
        ingredientList.Add(rice, 100);
        ingredientList.Add(pesto, 100);
        ingredientList.Add(chicken, 150);
        ingredientList.Add(shrimp, 150);
        ingredientList.Add(bun, 150);
        ingredientList.Add(cheese, 150);
        ingredientList.Add(steak, 200);
        ingredientList.Add(patty, 200);
        ingredientList.Add(flour, 200);
        //TODO
        ingredientList.Add(testIngredient, 200);
        #endregion

        #region add recipe scores to scoreSet

        scoreSet.Add(hamburgerRecipe, RecipeScore(hamburgerRecipe));
        scoreSet.Add(hotdogRecipe, RecipeScore(hotdogRecipe));
        scoreSet.Add(chefSaladRecipe, RecipeScore(chefSaladRecipe));
        scoreSet.Add(spaghettiCarbonaraRecipe, RecipeScore(spaghettiCarbonaraRecipe));
        scoreSet.Add(ratatouilleRecipe, RecipeScore(ratatouilleRecipe));
        scoreSet.Add(tenderloinSteakRecipe, RecipeScore(tenderloinSteakRecipe));
        scoreSet.Add(kidsMealRecipe, RecipeScore(kidsMealRecipe));
        scoreSet.Add(sandwichRecipe, RecipeScore(sandwichRecipe));
        scoreSet.Add(tempuraRecipe, RecipeScore(tempuraRecipe));
        scoreSet.Add(pumpkinPieRecipe, RecipeScore(pumpkinPieRecipe));
        scoreSet.Add(testRecipe, RecipeScore(testRecipe));
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        // should maybe change the following into a switch case statement
        #region check which dish is met, huge if statement
        if (hamburgerRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(hamburgerRecipe);
        }
        else if (sandwichRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(sandwichRecipe);
        }
        else if (tempuraRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(tempuraRecipe);
        }
        else if (pumpkinPieRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(pumpkinPieRecipe);
        }
        else if (ratatouilleRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(ratatouilleRecipe);
        }
        else if (spaghettiCarbonaraRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(spaghettiCarbonaraRecipe);
        }
        else if (chefSaladRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(chefSaladRecipe);
        } 
        else if (hotdogRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(hotdogRecipe);
        }  
        else if (tenderloinSteakRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(tenderloinSteakRecipe);
        }
        else if (kidsMealRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(kidsMealRecipe);
            //TODO
        } else if (testRecipe.IsSubsetOf(collectedItems.Keys))
        {
            MakeDish(testRecipe);
        }

        #endregion
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
                // not sure if this works
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
    }

}