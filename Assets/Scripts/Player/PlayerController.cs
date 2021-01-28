using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // TODO: Move these to Platform script and import from there
    const int NUMBER_OF_LANES = 3;
    const int LANE_LENGTH = 12;

    const float LANE_CHANGE_TIME = 0.05f;
    const float SLIDE_TIME = 2f;

    const float EPS = 0.01f;

    // Player Variables
    private Rigidbody _body;
    private float jumpHeight = 2f;
    [SerializeField]
    private float initialSpeed = 20f;
    private float currentSpeed;
    [SerializeField]
    private float failSpeed = 10f;      // Speed lower limit
    private int currentLane = 2;

    float movementTimeCount;
    float slideTimeCount;

    bool inMovement = false;
    bool isSliding = false;

    float starting_elevation;
    Vector3 shift;

    // Related to obstacle collisions
    private float speedReduction = 0f;
    private float collisionTime;
    [SerializeField]
    private float recoveryTime = 2f;    // Seconds after which player returns to previous speed
    private bool isInvincible = false;
    [SerializeField]
    private float invincibilityDuration = 1f;     // Seconds after which player is invincible against collisions

    void Start()
    {
        currentSpeed = initialSpeed;
        _body = gameObject.GetComponent<Rigidbody>();
        starting_elevation = _body.position.y;
    }


    void Update()
    {
        UpdateSpeed();
        if (inMovement == false && getIsNotJumpingOrSliding())
        {
            if (Input.GetButtonDown("Up")) {
                Jump();
            }
            if (Input.GetButtonDown("Down")) {
                Slide();
            }
            if (Input.GetButtonDown("Left")) {
                MoveLeft();
            }
            if (Input.GetButtonDown("Right")) {
                MoveRight();
            }
        }
        MoveForward();
        GoToDestination();
    }

    void Jump()
    {
        _body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    /*
     * TODO: Use isSliding to ignore collisions on obstacle scripts if they are elevated objects
     *       if (player.isSliding == true && obstacle.transform.position.y != 0.5f) { ignore collisions on obstacle }
     */
    void Slide() 
    {
        if (getIsNotJumpingOrSliding())
        {
            isSliding = true;
        }
    }

    void MoveForward()
    {
        _body.MovePosition(_body.position + (Time.deltaTime * new Vector3(0, 0, currentSpeed)));
    }

    void MoveLeft()
    {
        if (currentLane > 1)
        {
            inMovement = true;
            shift = new Vector3(-LANE_LENGTH / (NUMBER_OF_LANES + 1), 0, 0);
            currentLane--;
        }
    }

    void MoveRight()
    {
        if (currentLane < NUMBER_OF_LANES)
        {
            inMovement = true;
            shift = new Vector3(LANE_LENGTH / (NUMBER_OF_LANES + 1), 0, 0);
            currentLane++;
        }
    }

    void GoToDestination()
    {
        if (inMovement)
        {
            _body.MovePosition(_body.position + (Mathf.Min(LANE_CHANGE_TIME - movementTimeCount, Time.deltaTime) * shift / LANE_CHANGE_TIME));
            movementTimeCount += Time.deltaTime;
            if (movementTimeCount >= LANE_CHANGE_TIME)
            {
                inMovement = false;
                shift = Vector3.zero;
                movementTimeCount = 0;
            }
        }

        if (!inMovement && isSliding)
        {
            slideTimeCount += Time.deltaTime;
            if (slideTimeCount >= SLIDE_TIME)
            {
                isSliding = false;
                slideTimeCount = 0;
            }
        }
    }

    private void UpdateSpeed()
    {
        if (speedReduction != 0f) {
            currentSpeed -= speedReduction;
            speedReduction = 0f;
        }

        // TODO: Recover or increase speed

        if (currentSpeed < failSpeed) {
            // TODO: handle death
            Debug.Log("Too slow, should die.");
        }
    }

    private IEnumerator BecomeInvincibleTemporary()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += Time.deltaTime) {          
            // TODO: Can add visual cues for invincibility  
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isInvincible = false;
    }

    bool getIsNotJumpingOrSliding()
    {
        return (_body.position.y <= starting_elevation + EPS) && (starting_elevation - EPS <= _body.position.y);
    }

    public void SlowDown(float reduction) {
        if (!isInvincible) {
            speedReduction = reduction;
            collisionTime = Time.time;
            StartCoroutine(BecomeInvincibleTemporary());
        }
    }
}

