using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour
{
    [SerializeField]
    private float criticVisibleMinimumDistanceDifference = 3.0f;
    [SerializeField]
    private float criticVisibleSpeedDifference = 5.0f;

    private Rigidbody _body;
    private PlayerController playerController;

    void Start()
    {
        _body = gameObject.GetComponent<Rigidbody>();
        playerController = this.GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (playerController != null)
        {
            Vector3 parentPosition = this.transform.parent.position;

            float gameOverSpeedRemaining = playerController.GetCurrentSpeed() - playerController.gameOverSpeed;
            if (gameOverSpeedRemaining <= criticVisibleSpeedDifference)
            {
                float distanceBetween = (gameOverSpeedRemaining / 2);
                if (distanceBetween <= criticVisibleMinimumDistanceDifference)
                {
                    this.transform.position = new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - criticVisibleMinimumDistanceDifference);
                } 
                else
                {
                    this.transform.position = new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - distanceBetween);
                }
            }
        }

        MoveForward();
    }

    void MoveForward()
    {
        _body.MovePosition(_body.position - (Time.deltaTime * new Vector3(0, 0, playerController.GetCurrentSpeed())));
    }
}
