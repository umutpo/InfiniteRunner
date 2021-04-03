using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing a recipe on the UI with dish shown
public class RecipeUI : StackedRecipeUI
{
    void OnEnable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent += ChangeRecipeDisplayed;
    }

    void OnDisable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent -= ChangeRecipeDisplayed;
    }
    new void ChangeRecipeDisplayed() {
        base.ChangeRecipeDisplayed();
        RecipeController recipeToBeDisplayed = GetRecipeToDisplay();
        GameObject dishImageForeground = gameObject.transform.GetChild(0).gameObject;
        dishImageForeground.GetComponent<Image>().sprite = recipeToBeDisplayed.GetRecipeImage();
    }
}
