﻿using System.Collections;
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
    private Vector3 leftPosition;
    private Vector3 rightPosition;
    // TODO: change according to model
    private float kitchenLength = 20f;

    [SerializeField]
    private GameObject player;

    private string[] KITCHENS = {Pool.KITCHEN1, Pool.KITCHEN2, Pool.KITCHEN3, Pool.KITCHEN4, Pool.KITCHEN5, Pool.KITCHEN6};
    private Vector3[] pos = new Vector3[2];

    protected void Start()
    {
        kitchenCount = 0;
        leftPosition = transform.position + new Vector3(-9, -1, 0);
        rightPosition = transform.position + new Vector3(9, -1, 0);
        pos[0] = leftPosition;
        pos[1] = rightPosition;
        kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(), leftPosition, Quaternion.identity);
        if (kitchen != null)
        {
            leftPosition.z += kitchen.transform.localScale.z * kitchenLength;
        }
        kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(), rightPosition, Quaternion.identity);
        if (kitchen != null)
        {
            rightPosition.z += kitchen.transform.localScale.z * kitchenLength;
        }
    }

    protected void Update()
    {
        AddKitchen(0);
        AddKitchen(1);
    }

    private void AddKitchen(int posIndex) {
        float spawnRange = pos[posIndex].z - spawnDistance;        
        if (player.transform.position.z > spawnRange && kitchenCount < maximumKitchenCount)
        {
            // Set position of platform
            kitchen = ObjectPooler.Instance.SpawnFromPool(GetRandomKitchen(), pos[posIndex], Quaternion.identity);
            // Check if platform spawned
            if (kitchen == null) return;   // Should never occur if pool size is larger than max
            pos[posIndex].z += kitchen.transform.localScale.z * kitchenLength;

            kitchenComponent = kitchen.GetComponent<PlatformController>();
            kitchenComponent.OnObjectSpawn();
            kitchenComponent.onRemoveItem += RemoveOne;

            kitchenCount++;
        }
    }

    private string GetRandomKitchen() {
        int kit = Random.Range(0, KITCHENS.Length);
        return KITCHENS[kit];
    }

    
    void RemoveOne()
    {
        kitchenCount--;
    }
}