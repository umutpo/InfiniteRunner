using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// add on any gameobject to test. This code loops through all ingredients and adds 1 count to all of them, then subtracts 1 from tomato.
public class TestIngredientList : MonoBehaviour
{
    void Start()
    {
        foreach (Ingredient i in Ingredient.GetValues(typeof(Ingredient)))
        {
            IngredientList.AddIngredient(i);
        }
        IngredientList.RemoveIngredient(Ingredient.Tomato);
    }

    
}
