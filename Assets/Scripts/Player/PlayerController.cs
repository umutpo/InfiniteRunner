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
    const float PERMANENT_SPEED_GAIN_TIME = 60f;
    const float OBSTACLE_LOST_SPEED_GAIN_TIME = 3f;
    const float DISH_SPEED_GAIN_TIME = 2f;              // Time to regain speed for EACH ingredient used

    const float EPS = 0.01f;

    const float INITIAL_SPEED = 10f;
    const float PERMANENT_SPEED_GAIN = 1f;
    const float OBSTACLE_SPEED_GAIN = 1f;
    public const float INGREDIENT_SPEED_GAIN = 1f;

    // Input Variables
    public InputAction jumpAction;
    public InputAction slideAction;
    public InputAction moveLeftAction;
    public InputAction moveRightAction;

    // Player Variables
    private Rigidbody _body;
    private float jumpHeight = 2f;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float currentSpeed;
    public float gameOverSpeed = 2f;
    private int currentLane = 2;
    private float obstacleSpeedGainRemainder = 0f;
    private float dishSpeedGainRemainder = 0f;

    // Inventory Variables
    [SerializeField]
    private GameObject inventory;
    private PlayerInventoryData playerInventoryData;

    float movementTimeCount;
    float slideTimeCount;
    float permanentSpeedCount;
    float obstacleSpeedCount;
    float ingredientSpeedCount;

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
        maxSpeed = INITIAL_SPEED;
        currentSpeed = INITIAL_SPEED;
        jumpAction.performed += ctx => Jump();
        slideAction.performed += ctx => Slide();
        moveLeftAction.performed += ctx => MoveLeft();
        moveRightAction.performed += ctx => MoveRight();

        _body = gameObject.GetComponent<Rigidbody>();
        starting_elevation = _body.position.y;

        inventory = GameObject.Find("Inventory");
        playerInventoryData = inventory.GetComponent<PlayerInventoryData>();
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

        GainPermanentSpeed();
        GainLostSpeedFromObstacle();
        GainSpeedFromCreatingDish();
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
        if (_body != null)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
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

    void GainLostSpeedFromObstacle()
    {
        if (obstacleSpeedGainRemainder > 0)
        {
            obstacleSpeedCount += Time.deltaTime;
            if (obstacleSpeedCount >= OBSTACLE_LOST_SPEED_GAIN_TIME)
            {
                if (obstacleSpeedGainRemainder < OBSTACLE_SPEED_GAIN)
                {
                    currentSpeed += obstacleSpeedGainRemainder;
                    obstacleSpeedGainRemainder -= obstacleSpeedGainRemainder;
                }
                else
                {
                    currentSpeed += OBSTACLE_SPEED_GAIN;
                    obstacleSpeedGainRemainder -= OBSTACLE_SPEED_GAIN;
                }
                obstacleSpeedCount = 0;
            }
        }
    }

    void GainSpeedFromCreatingDish()
    {
        if (dishSpeedGainRemainder > 0)
        {
            ingredientSpeedCount += Time.deltaTime;
            if (ingredientSpeedCount >= DISH_SPEED_GAIN_TIME)
            {
                if (dishSpeedGainRemainder < INGREDIENT_SPEED_GAIN)
                {
                    currentSpeed += dishSpeedGainRemainder;
                    dishSpeedGainRemainder -= dishSpeedGainRemainder;
                }
                else
                {
                    currentSpeed += INGREDIENT_SPEED_GAIN;
                    dishSpeedGainRemainder -= INGREDIENT_SPEED_GAIN;
                }
                ingredientSpeedCount = 0;
            }            
        }
    }

    void GainPermanentSpeed()
    {
        permanentSpeedCount += Time.deltaTime;
        if (permanentSpeedCount >= PERMANENT_SPEED_GAIN_TIME)
        {
            maxSpeed += PERMANENT_SPEED_GAIN;
            currentSpeed += PERMANENT_SPEED_GAIN;
            permanentSpeedCount = 0;
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
        if (speedReduction != 0f)
        {
            currentSpeed -= speedReduction;
            speedReduction = 0f;
        }
    }

    private IEnumerator BecomeInvincibleTemporary()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += Time.deltaTime)
        {
            // TODO: Can add visual cues for invincibility  
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isInvincible = false;
    }

    bool getIsNotJumpingOrSliding()
    {
        return (_body.position.y <= starting_elevation + EPS) && (starting_elevation - EPS <= _body.position.y);
    }

    bool isGameOver()
    {
        if (currentSpeed <= gameOverSpeed)
        {
            return true;
        }

        return false;
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

    public void SlowDown(float reduction, bool isObstacle = true)
    {
        if (isObstacle)
        {
            if (!isInvincible)
            {
                speedReduction = (maxSpeed / reduction);
                obstacleSpeedGainRemainder += (maxSpeed / reduction);
                collisionTime = Time.time;
                StartCoroutine(BecomeInvincibleTemporary());
            }
        }
        else
        {
            speedReduction = reduction;
        }
    }

    public void AddToInventory(string ingredient, Sprite inventoryImage)
    {
        int usedIngredientCount = playerInventoryData.AddIngredient(ingredient, inventoryImage);
        if (usedIngredientCount > 0) {
            dishSpeedGainRemainder += usedIngredientCount * INGREDIENT_SPEED_GAIN;
        }
    }

    public void RemoveFromInventory(string ingredient)
    {
        playerInventoryData.RemoveIngredient(ingredient);
    }

    public double GetJumpDuration()
    {
        return 0;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}

