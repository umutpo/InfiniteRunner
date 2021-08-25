using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour, IPooledObject
{
    private const float ITEM_REMOVAL_DISTANCE = 10.0f;

    [SerializeField]
    protected GameObject player;
    protected PlayerController playerScript;

    public delegate void ItemDelegate();
    public ItemDelegate onRemoveItem;
  
    public virtual void OnObjectSpawn() {
    }

    protected virtual void Start() {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    protected virtual void Update() {
        if (player.transform.position.z > (transform.position.z + transform.localScale.z + ITEM_REMOVAL_DISTANCE))
            // Remove once out of camera view
            Remove();
    }

    protected virtual void OnTriggerEnter(Collider other) {
    }

    public virtual void Remove() {
        if (onRemoveItem != null)
        {
            onRemoveItem.Invoke();
        }
        onRemoveItem = null;
        gameObject.SetActive(false);
    }
}
