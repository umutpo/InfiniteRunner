using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    public string recipeName;
    [SerializeField]
    public List<IngredientController> ingredients = new List<IngredientController>();
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

    public Sprite GetRecipeImage() 
    {
        return inventoryImage;
    }

    private string getIngredientName(IngredientController ingredientController)
    {
        return ingredientController.ingredient;
    }
}
