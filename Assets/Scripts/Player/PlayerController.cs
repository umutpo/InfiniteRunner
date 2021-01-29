using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // TODO: Move these to Platform script and import from there
    const int NUMBER_OF_LANES = 3;
    const int LANE_LENGTH = 12;

    const float LANE_CHANGE_TIME = 0.05f;
    const float SLIDE_TIME = 2f;

    const float EPS = 0.01f;

    // Input Variables
    public InputAction jumpAction;
    public InputAction slideAction;
    public InputAction moveLeftAction;
    public InputAction moveRightAction;

    // Player Variables
    private Rigidbody _body;
    private float jumpHeight = 2f;
    [SerializeField]
    private float initialSpeed = 20f;
    private float currentSpeed;
    [SerializeField]
    private float failSpeed = 10f;      // Speed lower limit
    public float speed = 4f;
    public float gameOverSpeed = 0.1f;
    private int currentLane = 2;

    float movementTimeCount;
    float slideTimeCount;

    bool inMovement = false;
    bool isSliding = false;
    private bool gameOverState = false;

    float starting_elevation;
    Vector3 shift;

    // Related to obstacle collisions
    private float speedReduction = 0f;
    private float collisionTime;
    private bool isInvincible = false;
    [SerializeField]
    private float invincibilityDuration = 1f;     // Seconds after which player is invincible against collisions

    void Start()
    {
        currentSpeed = initialSpeed;
        jumpAction.performed += ctx => Jump();
        slideAction.performed += ctx => Slide();
        moveLeftAction.performed += ctx => MoveLeft();
        moveRightAction.performed += ctx => MoveRight();

        _body = gameObject.GetComponent<Rigidbody>();
        starting_elevation = _body.position.y;
    }


    void Update()
    {
        UpdateSpeed();
        if (inMovement == false && getIsNotJumpingOrSliding())
        {
            enableInputActions();
        }
        else
        {
            disableInputActions();
        }

        MoveForward();
        GoToDestination();
        if (isGameOver())
        {
            gameOverState = true;
            enabled = false;
        }
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

    void SlowDown(float reduction) {
        if (!isInvincible) {
            speedReduction = reduction;
            collisionTime = Time.time;
            StartCoroutine(BecomeInvincibleTemporary());
        }
    }
    
    bool isGameOver()
    {
        if (speed <= gameOverSpeed) {
            return true;
        } else { 
            return false;
        }
    }

    public bool getGameOverState()
    {
        return gameOverState;
    }

    void enableInputActions()
    {
        jumpAction.Enable();
        slideAction.Enable();
        moveLeftAction.Enable();
        moveRightAction.Enable();
    }

    void disableInputActions()
    {
        jumpAction.Disable();
        slideAction.Disable();
        moveLeftAction.Disable();
        moveRightAction.Disable();
    }
}

