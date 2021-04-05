using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    public string ingredient;
    private float speedReduction = 7f;  // ratio of speed reduced
    [SerializeField]
    private Sprite inventoryImageColored;
    [SerializeField]
    private Sprite inventoryImageGreyed = null;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.AddToInventory(ingredient);
            playerScript.SlowDown(speedReduction, false);
            Remove();
        }
    }
    public Sprite GetIngredientImageColored() 
    {
        return inventoryImageColored;
    }
    public Sprite GetIngredientImageGreyed() 
    {
        return inventoryImageGreyed;
    }
}
