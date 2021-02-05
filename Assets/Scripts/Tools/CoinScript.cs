using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public string ingredient;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            AddToCollected(other);
        }
    }


    private void AddToCollected(Collider other)
    {
        if (other.GetComponent<ItemCollection>().collectedItems.ContainsKey(ingredient))
        {
            int num;
            other.GetComponent<ItemCollection>().collectedItems.TryGetValue(ingredient, out num);
            other.GetComponent<ItemCollection>().collectedItems.Remove(ingredient);
            other.GetComponent<ItemCollection>().collectedItems.Add(ingredient, num + 1);
        }
        else
        {
            other.GetComponent<ItemCollection>().collectedItems.Add(ingredient, 1);
        }
    }
}