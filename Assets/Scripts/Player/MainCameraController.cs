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
    private float swipeDuration = 0.2f;
    private float deltaMovement = 0f;
    private float pastTimeSinceLastSwipe = 0f;
    void FixedUpdate()
    {
        pastTimeSinceLastSwipe += Time.fixedDeltaTime;
        if (target.position.x != transform.position.x && deltaMovement == 0)
        {
            deltaMovement = target.position.x - transform.position.x;
            pastTimeSinceLastSwipe = 0;
        }
        if (pastTimeSinceLastSwipe < swipeDuration)
        {
            transform.position = new Vector3(target.position.x - deltaMovement * (swipeDuration - pastTimeSinceLastSwipe)/swipeDuration , cameraElevation, target.position.z + cameraFollowDistance);
        }
        else
        {
            deltaMovement = 0;
            transform.position = new Vector3(target.position.x, cameraElevation, target.position.z + cameraFollowDistance);
        }
    }
}