using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    [SerializeField]
    public string recipeName;
    [SerializeField]
    public List<IngredientController> ingredients = new List<IngredientController>();
    [SerializeField]
    private Sprite inventoryImage;

    public List<string> getListOfIngredients()
    {
        return ingredients.ConvertAll<string>(getIngredientName);
    }

    private string getIngredientName(IngredientController ingredientController)
    {
        return ingredientController.ingredient;
    }
}
