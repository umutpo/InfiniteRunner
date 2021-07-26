using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraElevation = 8.0f;
    [SerializeField]
    private float cameraFollowDistance = -8.0f;

    [SerializeField]
    public Transform target;

    void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, cameraElevation, target.position.z + cameraFollowDistance);
    }
}