using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour
{
    [SerializeField]
    private float criticVisibleMinimumDistanceDifference = 1.0f;
    [SerializeField]
    private float criticVisibleSpeedDifference = 3.0f;

    private Rigidbody _body;
    private PlayerController playerController;
    private PlayerAudioController audioController;
    private Animator animator;

    private bool criticApproaching = false;

    void Start()
    {
        _body = gameObject.GetComponent<Rigidbody>();
        playerController = this.GetComponentInParent<PlayerController>();
        animator = gameObject.GetComponent<Animator>();
        audioController = GameObject.FindObjectOfType<PlayerAudioController>();
    }

    void Update()
    {
        if (playerController != null)
        {
            Vector3 parentPosition = this.transform.parent.position;

            float gameOverSpeedRemaining = playerController.GetCurrentSpeed() - playerController.GetGameOverSpeed();

            if (!criticApproaching && gameOverSpeedRemaining <= criticVisibleSpeedDifference)
            {
                audioController.PlayCriticApproaching();
            }
            criticApproaching = gameOverSpeedRemaining <= criticVisibleSpeedDifference;

            if (criticApproaching)
            {
                float distanceBetween = (gameOverSpeedRemaining / 2);
                if (distanceBetween <= criticVisibleMinimumDistanceDifference)
                {
                    this.transform.position = new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - criticVisibleMinimumDistanceDifference);
                    animator.SetInteger("CriticCloseness", 1);
                } 
                else
                {
                    this.transform.position = new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - distanceBetween);
                    animator.SetInteger("CriticCloseness", 0);
                }
            }
            else {
                animator.SetInteger("CriticCloseness", 0);
            }
        }

        MoveForward();
    }

    void MoveForward()
    {
        _body.MovePosition(_body.position - (Time.deltaTime * new Vector3(0, 0, playerController.GetCurrentSpeed())));
    }
}
