using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    public string ingredient;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.AddToInventory(ingredient);
            playerScript.SlowDown(PlayerController.INGREDIENT_SPEED_REDUCTION, false);
            Remove();
        }
    }
}
