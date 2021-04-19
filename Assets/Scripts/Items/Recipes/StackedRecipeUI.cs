using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach on an object representing a recipe on the UI with dish hidden
public class StackedRecipeUI : MonoBehaviour
{
    protected PlayerInventoryData playerInventoryData;
    
    protected int priority;
    [SerializeField] private List <Sprite> stackedDishTemplatesIncreasingCompletionRate; 

    void OnEnable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent += ChangeRecipeDisplayed;
    }

    void OnDisable()
    {
        PlayerInventoryData.UpdateRecipeUIEvent -= ChangeRecipeDisplayed;
    }

    private void Awake() {
        GameObject player = GameObject.Find("Inventory");
        if (player != null)
        {
            playerInventoryData = player.GetComponent<PlayerInventoryData>();
        }
        else 
        { 
            Debug.LogError("Cannot find inventory object named \' Inventory \' in the scene. Please change RecipeUI.cs if the inventory object was renamed."); 
        }

        if (playerInventoryData == null)
        {
            Debug.LogError("Cannot find \' PlayerInventoryData \' script on the \' Inventory \' object in the scene. Please change RecipeUI.cs if the inventory object was renamed, or reattach/rename \' PlayerInventoryData \'.");
        }
    }

    public void SetPriority(int setPriority) {
        priority = setPriority;
    }

    protected void ChangeRecipeDisplayed() {
        List <Dictionary <string, int> > recipeProgressList = new List <Dictionary <string, int> >(playerInventoryData.GetRecipeProgress());  
        RecipeController recipeToBeDisplayed = GetRecipeToDisplay();
        recipeProgressList.Sort(closestToFinishFirst);
        GameObject dishImageForeground = gameObject.transform.GetChild(0).gameObject;
        dishImageForeground.GetComponent<Image>().sprite = recipeToBeDisplayed.GetRecipeImage();
        int ingredientIterator = 0;
        int hasIngredientCnt = 0;
        foreach (Transform child in dishImageForeground.transform) {
            IngredientController currentIngredient = recipeToBeDisplayed.ingredients[ingredientIterator];
            if (recipeProgressList[priority][currentIngredient.ingredient] != 0)
                hasIngredientCnt++;
            ingredientIterator++;
        }
        // only set the UI sprite if on a stacked UI element
        if (stackedDishTemplatesIncreasingCompletionRate.Count == 3)
            gameObject.GetComponent<Image>().sprite = stackedDishTemplatesIncreasingCompletionRate[hasIngredientCnt];
    }
    protected RecipeController GetRecipeToDisplay() {
        List <RecipeController> recipes = playerInventoryData.GetRecipes();
        List <Dictionary <string, int> > recipeProgressList = new List <Dictionary <string, int> >(playerInventoryData.GetRecipeProgress());
        Dictionary<Dictionary <string, int>, RecipeController> dInverse = new Dictionary<Dictionary <string, int>, RecipeController>();
        for (int i = 0; i < recipeProgressList.Count; i++) 
        {
            dInverse[recipeProgressList[i]] = recipes[i];
        }
        recipeProgressList.Sort(closestToFinishFirst);
        return dInverse[recipeProgressList[priority]];
    }

    // recipe priority comparison rule; whichever needs the least number of ingredients to complete gets placed foremost
    protected int closestToFinishFirst(Dictionary<string, int> recipe1, Dictionary<string, int> recipe2)
    {
        int missingIngredientCnt1 = 0;
        int missingIngredientCnt2 = 0;
        foreach (KeyValuePair<string, int> ingredientAmount in recipe1)
        {
            if (ingredientAmount.Value == 0)
            {
                missingIngredientCnt1++;
            }
        }
        foreach (KeyValuePair<string, int> ingredientAmount in recipe2)
        {
            if (ingredientAmount.Value == 0)
            {
                missingIngredientCnt2++;
            }
        }

        if (missingIngredientCnt1 == missingIngredientCnt2)
        {
            return 1;
        }
        else
        {
            return missingIngredientCnt1 - missingIngredientCnt2;
        }
    }
}
