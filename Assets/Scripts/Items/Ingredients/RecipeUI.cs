using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing an ingredient on the UI with a text component whose value is the current count of the ingredient in inventory
public class RecipeUI : MonoBehaviour
{
    private PlayerInventoryData playerInventoryData;
    private int priority;

    void OnEnable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent += ChangeRecipeDisplayed;
    }

    void OnDisable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent -= ChangeRecipeDisplayed;
    }
    private void Start() {
        GameObject player = GameObject.Find("Inventory");
        if (player != null)
            playerInventoryData = player.GetComponent<PlayerInventoryData>();
        else Debug.LogError("Cannot find inventory object named \' Inventory \' in the scene. Please change RecipeUI.cs if the inventory object was renamed.");
        if (playerInventoryData == null)
            Debug.LogError("Cannot find \' PlayerInventoryData \' script on the \' Inventory \' object in the scene. Please change RecipeUI.cs if the inventory object was renamed, or reattach/rename \' PlayerInventoryData \'.");
    }

    public void SetPriority(int setPriority) {
        priority = setPriority;
    }

    void ChangeRecipeDisplayed() {
        List <RecipeController> recipes = playerInventoryData.GetRecipes();
        List <Dictionary <string, int> > recipeProgressList = new List <Dictionary <string, int> >(playerInventoryData.GetRecipeProgress());
        Dictionary<Dictionary <string, int>, RecipeController> dInverse = new Dictionary<Dictionary <string, int>, RecipeController>();
        for (int i = 0; i < recipeProgressList.Count; i++) 
        {
            dInverse[recipeProgressList[i]] = recipes[i];
        }
        recipeProgressList.Sort(closestToFinishFirst);
        RecipeController recipeToBeDisplayed = dInverse[recipeProgressList[priority]];
        GameObject dishImageForeground = gameObject.transform.GetChild(0).gameObject;
        dishImageForeground.GetComponent<Image>().sprite = recipeToBeDisplayed.GetRecipeImage();

        int ingredientIterator = 0;
        foreach (Transform child in dishImageForeground.transform) {
            IngredientController currentIngredient = recipeToBeDisplayed.ingredients[ingredientIterator];
            if (recipeProgressList[priority][currentIngredient.ingredient] != 0)
                child.GetChild(0).GetComponent<Image>().sprite = currentIngredient.GetIngredientImage();
            else
                child.GetChild(0).GetComponent<Image>().sprite = null;
            ingredientIterator++;
        }
    }

    // recipe priority comparison rule; whichever needs the least number of ingredients to complete gets placed foremost
    private int closestToFinishFirst(Dictionary<string, int> recipe1, Dictionary<string, int> recipe2)
    {
        int missingIngredientCnt1 = 0;
        int missingIngredientCnt2 = 0;
        foreach (KeyValuePair<string, int> ingredientAmount in recipe1)
        {
            if (ingredientAmount.Value == 0)
                missingIngredientCnt1++;
        }
        foreach (KeyValuePair<string, int> ingredientAmount in recipe2)
        {
            if (ingredientAmount.Value == 0)
                missingIngredientCnt2++;
        }
        if (missingIngredientCnt1 == missingIngredientCnt2)
            return 1;
        else
            return missingIngredientCnt1 - missingIngredientCnt2;
    }
}
