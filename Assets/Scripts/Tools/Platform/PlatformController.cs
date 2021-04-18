using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour, IPooledObject
{    
    [SerializeField]
    protected GameObject player;

    public delegate void PlatformDelegate();
    public PlatformDelegate onRemovePlatform;
  
    public virtual void OnObjectSpawn() {
    }

    protected virtual void Start() {
        player = GameObject.Find("Player");
    }

    protected virtual void Update() {
        // Remove once out of camera view
        if (player.transform.position.z > transform.position.z + transform.localScale.z)
            Remove();
    }

    public virtual void Remove() {
        if (onRemovePlatform != null)
        {
            onRemovePlatform.Invoke();
        }
        onRemovePlatform = null;
        gameObject.SetActive(false);
    }
}
