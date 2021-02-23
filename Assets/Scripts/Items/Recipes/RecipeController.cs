using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    [SerializeField]
    public List<IngredientController> ingredients = new List<IngredientController>();

    public List<string> getListOfIngredients()
    {
        return ingredients.ConvertAll<string>(getIngredientName);
    }

    private string getIngredientName(IngredientController ingredientController)
    {
        return ingredientController.ingredient;
    }
}
