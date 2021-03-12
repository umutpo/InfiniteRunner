using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    [SerializeField]
    private List<IngredientController> ingredients = new List<IngredientController>();
    [SerializeField]
    private Sprite inventoryImage;
    [SerializeField]
    private int points;

    public int GetRecipePoints()
    {
        return points;
    }

    public List<string> getListOfIngredients()
    {
        return ingredients.ConvertAll<string>(getIngredientName);
    }

    private string getIngredientName(IngredientController ingredientController)
    {
        return ingredientController.ingredient;
    }
}
