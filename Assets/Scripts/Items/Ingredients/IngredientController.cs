using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    protected string ingredientName = "Ingredient";

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.AddToInventory(ingredientName);
            playerScript.SlowDown(PlayerController.INGREDIENT_SPEED_REDUCTION, false);
            Remove();
        }
    }
}
