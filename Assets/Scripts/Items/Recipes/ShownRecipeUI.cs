using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing a recipe on the UI with dish shown
public class ShownRecipeUI : StackedRecipeUI
{
    void OnEnable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent += ChangeRecipeDisplayed;
    }

    void OnDisable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent -= ChangeRecipeDisplayed;
    }
}
