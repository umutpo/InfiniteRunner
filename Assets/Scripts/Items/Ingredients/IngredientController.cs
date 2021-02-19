using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    public string ingredient;
    [SerializeField]
    private Sprite inventoryImage;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.AddToInventory(ingredient, inventoryImage);
            playerScript.SlowDown(PlayerController.INGREDIENT_SPEED_GAIN, false);
            Remove();
        }
    }
}
