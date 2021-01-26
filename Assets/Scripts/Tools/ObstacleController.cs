using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour, IPooledObject
{    
    [SerializeField]
    private GameObject player;

    public delegate void ObstacleDelegate();
    public ObstacleDelegate onRemoveObstacle;
  
    public virtual void OnObjectSpawn() {
    }

    protected virtual void Start() {
        player = GameObject.Find("Player");
    }

    protected virtual void Update() {
        if (false) {
            // Remove and slow down player if collided
        } else if (player.transform.position.z > transform.position.z + transform.localScale.z)
            // Remove once out of camera view
            Remove();
    }

    public virtual void Remove() {
        if (onRemoveObstacle != null)
        {
            onRemoveObstacle.Invoke();
        }
        onRemoveObstacle = null;
        gameObject.SetActive(false);
    }
}
