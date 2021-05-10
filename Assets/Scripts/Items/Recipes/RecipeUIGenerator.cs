using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUIGenerator : MonoBehaviour
{
    [SerializeField]
    private int visibleRecipeCnt;

    [SerializeField]
    private GameObject recipeUITemplate;

    [SerializeField]
    private GameObject stackedRecipeUITemplate;

    void Awake()
    {
        GameObject inventory = GameObject.Find("Inventory");
        if (inventory == null)
        {
            Debug.LogError("Inventory object does not exist! Make sure to add an object named inventory with the PlayerInventoryData component attached.");
        }

        int recipeNumber = inventory.GetComponent<PlayerInventoryData>().GetRecipeNumber();
        for (int i = 0; i < recipeNumber - visibleRecipeCnt; i++) {
            GameObject g = Instantiate(stackedRecipeUITemplate, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(this.gameObject.transform, false);
            g.transform.GetChild(0).gameObject.GetComponent<StackedRecipeUI>().SetPriority(recipeNumber - i - 1);
        }

        for (int i = 0; i < visibleRecipeCnt; i++) {
            GameObject g = Instantiate(recipeUITemplate, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(this.gameObject.transform, false);
            g.transform.GetChild(0).gameObject.GetComponent<ShownRecipeUI>().SetPriority(visibleRecipeCnt - i - 1);
        }
    }
}
