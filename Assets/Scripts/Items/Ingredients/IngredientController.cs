using UnityEngine;
using System.Collections;

public class IngredientController : ItemController
{
    [SerializeField]
    public string ingredient;

    private float speedReduction = 10f;  // ratio of speed reduced
    private float rotationDegree = 90f; // degrees per second

    [SerializeField]
    private Sprite inventoryImageColored;

    [SerializeField]
    private Sprite inventoryImageGreyed = null;

    protected override void Update()
    {
        base.Update();
        transform.Rotate(0, rotationDegree * Time.deltaTime, 0, Space.World);
    }

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
