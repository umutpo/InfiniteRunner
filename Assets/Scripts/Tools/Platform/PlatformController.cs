using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour, IPooledObject
{    
    [SerializeField]
    private GameObject player;
    // public int counterChoices;

    private Transform leftPlat;
    private Transform rightPlat;
    private GameObject leftTop;
    private GameObject rightTop;

    public delegate void PlatformDelegate();
    public PlatformDelegate onRemovePlatform;
  
    public virtual void OnObjectSpawn() {
    }

    protected virtual void Awake() {
        player = GameObject.Find("Player");
        rightPlat = this.gameObject.transform.GetChild(0);
        leftPlat = this.gameObject.transform.GetChild(1);
        // counterChoices = leftPlat.childCount;
    }

    protected virtual void Update() {
        // Remove once out of camera view
        if (player.transform.position.z > transform.position.z + transform.localScale.z)
            Remove();
    }

    public virtual void SetCounter(int counterLeft, int counterRight) {
        // Spawn with set counter
        rightTop = rightPlat.GetChild(counterRight).gameObject;
        leftTop = leftPlat.GetChild(counterLeft).gameObject;
        rightTop.SetActive(true);
        leftTop.SetActive(true);
    }

    public virtual void Remove() {
        if (onRemovePlatform != null)
        {
            onRemovePlatform.Invoke();
        }
        onRemovePlatform = null;
        if (leftTop != null) leftTop.SetActive(false);
        if (rightTop != null) rightTop.SetActive(false);
        gameObject.SetActive(false);
    }
}
