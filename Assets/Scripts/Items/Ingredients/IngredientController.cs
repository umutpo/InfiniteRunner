using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    public string ingredient;
    private float speedReduction = 7f;  // ratio of speed reduced
    [SerializeField]
    private Sprite inventoryImage;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.AddToInventory(ingredient, inventoryImage);
            playerScript.SlowDown(speedReduction, false);
            Remove();
        }
    }
    public Sprite GetIngredientImage() 
    {
        return inventoryImage;
    }
}
