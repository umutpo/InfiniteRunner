using UnityEngine;
using System.Collections;

public class IngredientController : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private GameObject player;
    private PlayerController playerScript;

    public delegate void IngredientDelegate();
    public IngredientDelegate onRemoveIngredient;

    public virtual void OnObjectSpawn()
    {
    }

    protected virtual void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {
        if (player.transform.position.z > transform.position.z + transform.localScale.z)
            // Remove once out of camera view
            Remove();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Remove();
        }
    }

    public virtual void Remove()
    {
        if (onRemoveIngredient != null)
        {
            onRemoveIngredient.Invoke();
        }
        onRemoveIngredient = null;
        gameObject.SetActive(false);
    }
}
