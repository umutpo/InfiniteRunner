using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing a recipe on the UI with dish shown
public class ShownRecipeUI : StackedRecipeUI
{
    [SerializeField] private List <Sprite> shownDishTemplatesIncreasingCompletionRate;
    void OnEnable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent += ChangeRecipeDisplayed;
    }

    void OnDisable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent -= ChangeRecipeDisplayed;
    }

    new void ChangeRecipeDisplayed() 
    {
        base.ChangeRecipeDisplayed();
        List <Dictionary <string, int> > recipeProgressList = new List <Dictionary <string, int> >(playerInventoryData.GetRecipeProgress());  
        RecipeController recipeToBeDisplayed = GetRecipeToDisplay();
        recipeProgressList.Sort(closestToFinishFirst);
        GameObject dishImageForeground = gameObject.transform.GetChild(0).gameObject;
        int ingredientIterator = 0;
        int hasIngredientCnt = 0;
        foreach (Transform child in dishImageForeground.transform) {
            IngredientController currentIngredient = recipeToBeDisplayed.ingredients[ingredientIterator];
            if (recipeProgressList[priority][currentIngredient.ingredient] != 0)
            {
                child.GetChild(0).GetComponent<Image>().sprite = currentIngredient.GetIngredientImageColored();
                hasIngredientCnt++;
            }
            else
            {
                child.GetChild(0).GetComponent<Image>().sprite = currentIngredient.GetIngredientImageGreyed();
            }
            ingredientIterator++;
        }
        gameObject.GetComponent<Image>().sprite = shownDishTemplatesIncreasingCompletionRate[hasIngredientCnt];
    }
}
