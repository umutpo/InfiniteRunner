using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing an ingredient on the UI with a text component whose value is the current count of the ingredient in inventory
public class RecipeUI : MonoBehaviour
{
    [SerializeField] private string ingredientType;
    [SerializeField] private GameObject inventoryItemCountPrefab;
    [SerializeField] private GameObject inventoryImagePrefab;
    private GameObject inventoryItemCount;
    private GameObject inventoryImage;
    void OnEnable()
    {
        PlayerInventoryData.AddIngredientEvent += AddIngredient;
        PlayerInventoryData.RemoveIngredientEvent += RemoveIngredient;
    }

    void OnDisable()
    {
        PlayerInventoryData.AddIngredientEvent -= AddIngredient;
        PlayerInventoryData.RemoveIngredientEvent -= RemoveIngredient;
    }
    void Start() {
        inventoryItemCount = Instantiate(inventoryItemCountPrefab, Vector3.zero, Quaternion.identity);
        inventoryItemCount.transform.SetParent(gameObject.transform, false);
        inventoryImage = Instantiate(inventoryImagePrefab, Vector3.zero, Quaternion.identity);
        inventoryImage.transform.SetParent(gameObject.transform, false);
    }
    void AddIngredient(string ing, int count, Sprite curInventoryImage)
    {
        if (ingredientType.Equals(ing))
        {
            UpdateCount(count);
            if (count >= 1)
                inventoryImage.GetComponent<Image>().sprite = curInventoryImage;
        }
    }

    void RemoveIngredient(string ing, int count)
    {
        if (ingredientType.Equals(ing))
        {
            UpdateCount(count);
            if (count == 0)
                inventoryImage.GetComponent<Image>().sprite = null;
        }
    }

    void UpdateCount(int count)
    {
        inventoryItemCount.GetComponent<Text>().text = count.ToString();
    }
}
