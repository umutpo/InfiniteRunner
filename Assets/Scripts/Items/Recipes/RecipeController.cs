using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    [SerializeField]
    public List<IngredientController> ingredients = new List<IngredientController>();

    public List<string> getListOfIngredients()
    {
        List<string> result = new List<string>();
        foreach(IngredientController ing in ingredients)
        {
            result.Add(ing.ingredient);
        }
        return result;
    }
}
