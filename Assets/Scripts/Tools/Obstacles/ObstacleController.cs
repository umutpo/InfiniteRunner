using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour, IPooledObject
{    
    [SerializeField]
    private GameObject player;
    private PlayerController playerScript;
    [SerializeField]
    protected float speedReduction = 1f;

    public delegate void ObstacleDelegate();
    public ObstacleDelegate onRemoveObstacle;
  
    public virtual void OnObjectSpawn() {
    }

    protected virtual void Start() {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    protected virtual void Update() {
        if (player.transform.position.z > transform.position.z + transform.localScale.z)
            // Remove once out of camera view
            Remove();
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // Slow down player speed
            playerScript.SlowDown(speedReduction);
            Remove();
        }
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
