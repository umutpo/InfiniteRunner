using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour
{
    const float CRITIC_VISIBLE_SPEED_DIFFERENCE = 5.0f;

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
            if (gameOverSpeedRemaining <= CRITIC_VISIBLE_SPEED_DIFFERENCE)
            {
                this.transform.position =  new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - (gameOverSpeedRemaining / 2));
            }
        }

        MoveForward();
    }

    void MoveForward()
    {
        _body.MovePosition(_body.position - (Time.deltaTime * new Vector3(0, 0, playerController.GetCurrentSpeed())));
    }
}
