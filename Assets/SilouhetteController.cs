using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilouhetteController : MonoBehaviour
{
    [SerializeField]
    private float silouhetteElevation = 2.0f;
    [SerializeField]
    private float silouhetteDistance = 50.0f;

    [SerializeField]
    public Transform target;

    void FixedUpdate()
    {
        transform.position = new Vector3(0.0f, silouhetteElevation, target.position.z + silouhetteDistance);
    }
}