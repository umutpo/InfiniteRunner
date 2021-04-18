using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    private int kitchenCount;
    [SerializeField]
    private int maximumKitchenCount = 8;
    
    private GameObject kitchen;
    [SerializeField]
    private float spawnDistance = 200;
    private PlatformController kitchenComponent;
    private Vector3 kitchenPosition;

    [SerializeField]
    private GameObject player;

    private string[] KITCHENS = {Pool.KITCHEN1, Pool.KITCHEN2, Pool.KITCHEN3, Pool.KITCHEN4, Pool.KITCHEN5, Pool.KITCHEN6};

    protected void Start()
    {
        kitchenCount = 0;
        kitchenPosition = transform.position;
        kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(), kitchenPosition, Quaternion.identity);
        if (kitchen != null)
        {
            kitchenPosition.z += kitchen.transform.localScale.z * 20f;
        }
    }

    protected void Update()
    {
        float spawnRange = kitchenPosition.z - spawnDistance;
        if (player.transform.position.z > spawnRange && kitchenCount < maximumKitchenCount)
        {
            // Set position of platform
            kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(), kitchenPosition, Quaternion.identity);
            // Check if platform spawned
            if (kitchen == null) return;   // Should never occur if pool size is larger than max
            kitchenPosition.z += kitchen.transform.localScale.z * 20f;

            kitchenComponent = kitchen.GetComponent<PlatformController>();
            kitchenComponent.OnObjectSpawn();
            kitchenComponent.onRemovePlatform += RemoveOne;

            kitchenCount++;
        }
    }

    private string GetRandomKitchen() {
        int kit = Random.Range(0, 6);
        return KITCHENS[kit];
    }

    
    void RemoveOne()
    {
        kitchenCount--;
    }
}
