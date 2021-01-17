using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour, IPooledObject
{
    [SerializeField]
    public float lifeTime = 5;
    private float startTime;

    public delegate void PlatformDelegate();
    public PlatformDelegate onRemovePlatform;

    

    public virtual void OnObjectSpawn() {
        startTime = Time.time;
    }

    protected virtual void Start(){

    }

    protected virtual void Update() {
        // if (transform.position.z < camera.transform.position.z - 10) {
        //     Remove();
        // }
        // TODO: Remove once out of camera view
        if (Time.time > startTime + lifeTime)
            Remove();
    }

    public virtual void Remove() {
        onRemovePlatform?.Invoke();
        onRemovePlatform = null;
        gameObject.SetActive(false);
    }
}
