using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUIGenerator : MonoBehaviour
{
    [SerializeField] 
    private int visibleRecipeCnt;
    [SerializeField] 
    private GameObject recipeUITemplate;
    
    void Start()
    {
        for (int i = 0; i < visibleRecipeCnt; i++) {
            GameObject g = Instantiate(recipeUITemplate, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(this.gameObject.transform, false);
            g.GetComponent<RecipeUI>().SetPriority(i);
        }
    }
}
