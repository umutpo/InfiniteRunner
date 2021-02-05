using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    protected string ingredient = "test";

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.AddToInventory(ingredient);
            playerScript.SlowDown(PlayerController.INGREDIENT_SPEED_REDUCTION, false);
            AddToCollected(other);
            Remove();
        }
    }

    private void AddToCollected(Collider other)
    {
        if (other.GetComponent<ItemCollection>().collectedItems.ContainsKey(ingredient))
        {
            int num;
            other.GetComponent<ItemCollection>().collectedItems.TryGetValue(ingredient, out num);
            other.GetComponent<ItemCollection>().collectedItems.Remove(ingredient);
            other.GetComponent<ItemCollection>().collectedItems.Add(ingredient, num + 1);
        }
        else
        {
            other.GetComponent<ItemCollection>().collectedItems.Add(ingredient, 1);
        }
    }
}
