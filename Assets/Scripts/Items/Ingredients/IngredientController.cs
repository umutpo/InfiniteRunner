using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    protected string ingredient;

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
        if (other.GetComponent<PlayerInventoryData>().collectedItems.ContainsKey(ingredient))
        {
            int num;
            other.GetComponent<PlayerInventoryData>().collectedItems.TryGetValue(ingredient, out num);
            other.GetComponent<PlayerInventoryData>().collectedItems.Remove(ingredient);
            other.GetComponent<PlayerInventoryData>().collectedItems.Add(ingredient, num + 1);
        }
        else
        {
            other.GetComponent<PlayerInventoryData>().collectedItems.Add(ingredient, 1);
        }
    }
}
