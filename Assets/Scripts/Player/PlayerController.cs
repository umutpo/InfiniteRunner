using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    const int NUMBER_OF_LANES = 3;
    const int LANE_LENGTH = 12;

    const float LANE_CHANGE_TIME = 0.05f;
    const float SLIDE_TIME = 0.5f;
    const float PERMANENT_SPEED_GAIN_TIME = 60f;
    const float OBSTACLE_LOST_SPEED_GAIN_TIME = 3f;
    const float DISH_SPEED_GAIN_TIME = 3f;
    const float BOOST_DEACCEL_TIME = 0.5f;

    const float INITIAL_SPEED = 10f;
    const float PERMANENT_SPEED_GAIN = 1f;
    const float OBSTACLE_SPEED_GAIN = 1f;
    public const float INGREDIENT_SPEED_GAIN = 1f;
    const float DISH_SPEED_BOOST = 1f;

    const float EPS = 0.01f;

    const float INGREDIENT_WEIGHT = 1f;
    const float MAX_INVENTORY_INGREDIENT_WEIGHT = 10f;
    
    const float MAX_JUMP_HEIGHT = 3f;
    const float MAX_JUMP_DISTANCE = 10f;

    // Input Variables
    public InputAction jumpAction;
    public InputAction slideAction;
    public InputAction moveLeftAction;
    public InputAction moveRightAction;

    // Player Variables
    private Rigidbody _body;
    private BoxCollider _collider;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float currentSpeed;
    public float gameOverSpeed = 4f;
    private int currentLane = 2;
    private float obstacleSpeedGainRemainder = 0f;
    private bool inMovement = false;
    private bool isSliding = false;
    private bool gameOverState = false;
    private float extraWeight = 0f;
    private float starting_elevation;
    private Vector3 shift;

    // Inventory Variables
    [SerializeField]
    private GameObject inventory;
    private PlayerInventoryData playerInventoryData;

    // Timers
    float movementTimeCount;
    float slideTimeCount;
    float permanentSpeedCount;
    float obstacleSpeedCount;
    float ingredientSpeedCount;

    // Obstacle Collisions
    private float speedReduction = 0f;
    private bool setBoost = false;
    private bool isInvincible = false;
    [SerializeField]
    private float invincibilityDuration = 1f;

    // Animation Variables
    private Animator anim;

    // Tutorial Variables
    public static Action StopTutorial;
    private bool waitForTutorial = false;

    void Start()
    {
        maxSpeed = INITIAL_SPEED;
        currentSpeed = INITIAL_SPEED;

        jumpAction.performed += ctx => jump();
        slideAction.performed += ctx => slide();
        moveLeftAction.performed += ctx => moveLeft();
        moveRightAction.performed += ctx => moveRight();

        _body = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<BoxCollider>();
        starting_elevation = _body.transform.position.y;

        inventory = GameObject.Find("Inventory");
        playerInventoryData = inventory.GetComponent<PlayerInventoryData>();

        anim = gameObject.GetComponent<Animator>();
        
        moveLeftAction.Enable();
        moveRightAction.Enable();
    }

    void Update()
    {
        checkGameOver();
    }

    private void FixedUpdate()
    {
        if (canPlayerMove())
        {
            jumpAction.Enable();
            slideAction.Enable();
        }
        else
        {
            jumpAction.Disable();
            if (isPlayerGrounded())
            {
                slideAction.Disable();
            }
        }

        if (!waitForTutorial)
        {
            updateSpeed();
            moveBody();
        }
    }

    private void updateSpeed()
    {
        if (speedReduction != 0f)
        {
            currentSpeed -= speedReduction;
            speedReduction = 0f;
        }

        gainPermanentSpeed();
        gainLostSpeedFromObstacle();
        boostSpeedFromCreatingDish();
    }

    private void moveBody()
    {
        _body.MovePosition(_body.position + (Time.deltaTime * new Vector3(0, 0, currentSpeed)));

        if (isSliding)
        {
            slideTimeCount += Time.deltaTime;
            if (slideTimeCount >= SLIDE_TIME)
            {
                _collider.size = new Vector3(_collider.size.x, _collider.size.y * 2, _collider.size.z);
                _collider.center = new Vector3(_collider.center.x, 0f, _collider.center.z);
                slideTimeCount = 0;
                isSliding = false;
            }
        }

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
    }

    private void checkGameOver()
    {
        if (currentSpeed <= gameOverSpeed)
        {
            SetGameOver();
        }
    }

    public void SetGameOver()
    {
        gameOverState = true;
        enabled = false;
    }

    private void jump()
    {
        if (_body != null)
        {
            // TODO: Slide function has to be added for higher speed problems
            float time = currentSpeed / MAX_JUMP_DISTANCE;
            Physics.gravity = Vector3.up * -1 * ((2 * MAX_JUMP_HEIGHT) / Mathf.Pow((time / 2), 2));
            float verticalJumpSpeed = Physics.gravity.y * -1 * (time / 2);
            _body.AddForce(Vector3.up * verticalJumpSpeed, ForceMode.VelocityChange);
            playPLayerAnimation("Jump");
        }
    }

    private void slide()
    {
        if (!isSliding && _collider != null)
        {
            _body.AddForce(Vector3.down * 10, ForceMode.VelocityChange);
            _collider.size = new Vector3(_collider.size.x, _collider.size.y / 2, _collider.size.z);
            _collider.center = new Vector3(_collider.center.x, -1 * (_collider.size.y / 2), _collider.center.z);
            isSliding = true;
            playPLayerAnimation("Slide");
        }
    }

    private void moveLeft()
    {
        if (currentLane > 1)
        {
            inMovement = true;
            shift = new Vector3(-LANE_LENGTH / (NUMBER_OF_LANES + 1), 0, 0);
            currentLane--;
            playPLayerAnimation("MoveLeft");
        }
    }

    private void moveRight()
    {
        if (currentLane < NUMBER_OF_LANES)
        {
            inMovement = true;
            shift = new Vector3(LANE_LENGTH / (NUMBER_OF_LANES + 1), 0, 0);
            currentLane++;
            playPLayerAnimation("MoveRight");
        }
    }

    private void gainLostSpeedFromObstacle()
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

    private IEnumerator boostSpeed(float speedGain)
    {
        isInvincible = true;
        currentSpeed += speedGain;
        // Consider changing this time for another variable
        for (float i = 0; i < DISH_SPEED_GAIN_TIME; i += Time.deltaTime)
        {
            // TODO: Can add visual cues for invincibility  
            yield return new WaitForSeconds(Time.deltaTime);
        }
        float remaining = speedGain;
        for (float i = 0; i < BOOST_DEACCEL_TIME; i += Time.deltaTime)
        {
            float deaccel = Time.deltaTime * speedGain / BOOST_DEACCEL_TIME;
            currentSpeed -= deaccel;
            remaining -= deaccel;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        currentSpeed -= remaining;
        isInvincible = false;
    }

    private void boostSpeedFromCreatingDish()
    {
        if (setBoost) {
            currentSpeed = calculateExtraWeightSpeedDecreaseRatio() * maxSpeed;
            StartCoroutine(boostSpeed(DISH_SPEED_BOOST));
            setBoost = false;
        }
    }

    private void gainPermanentSpeed()
    {
        permanentSpeedCount += Time.deltaTime;
        if (permanentSpeedCount >= PERMANENT_SPEED_GAIN_TIME)
        {
            maxSpeed += PERMANENT_SPEED_GAIN;
            currentSpeed += PERMANENT_SPEED_GAIN;
            permanentSpeedCount = 0;
        }
    }

    private IEnumerator becomeInvincibleTemporary()
    {
        isInvincible = true;
        for (float i = 0; i < invincibilityDuration; i += Time.deltaTime)
        {
            // TODO: Can add visual cues for invincibility
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isInvincible = false;
    }

    private bool canPlayerMove()
    {
        return inMovement == false && isPlayerGrounded();
    }

    private bool isPlayerGrounded()
    {
        return (_body.position.y <= starting_elevation + EPS) && (starting_elevation - EPS <= _body.position.y);
    }

    public bool GetGameOverState()
    {
        return gameOverState;
    }

    public void SlowDown(float reduction, bool isObstacle = true)
    {
        if (isObstacle)
        {
            if (!isInvincible)
            {
                speedReduction = (maxSpeed / reduction);
                obstacleSpeedGainRemainder += (maxSpeed / reduction);
                StartCoroutine(becomeInvincibleTemporary());
            }
            playPLayerAnimation("Hit");
        }
        else
        {
            speedReduction = (maxSpeed / reduction);
        }
    }

    private void addToExtraWeight(int numberOfIngredient)
    {
        extraWeight += numberOfIngredient * INGREDIENT_WEIGHT;
    }

    private void reduceExtraWeight(int usedIngredientCount)
    {
        extraWeight -= usedIngredientCount * INGREDIENT_WEIGHT;
    }

    private float calculateExtraWeightSpeedDecreaseRatio()
    {
        return (MAX_INVENTORY_INGREDIENT_WEIGHT - extraWeight) / MAX_INVENTORY_INGREDIENT_WEIGHT;
    }

    private void playPLayerAnimation(string animationEventName)
    {
        if (anim != null)
        {
            anim.SetTrigger(animationEventName);
        }
    }

    public void AddToInventory(string ingredient)
    {
        addToExtraWeight(1);
        int usedIngredientCount = playerInventoryData.AddIngredient(ingredient);
        if (usedIngredientCount > 0) {
            setBoost = true;
            reduceExtraWeight(usedIngredientCount);
            playPLayerAnimation("Complete");
        } 
        else
        {
            playPLayerAnimation("Collect");
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public Dictionary<string, int> GetCollectedIngredientsCounts()
    {
        return playerInventoryData.GetCollectedIngredientsCounts();
    }

    public List<RecipeController> GetRecipes()
    {
        return playerInventoryData.GetRecipes();
    }

    public IEnumerator StartTutorial(UnityEngine.InputSystem.Controls.KeyControl key)
    {
        anim.enabled = false;
        waitForTutorial = true;
        yield return new WaitUntil(() => (key.isPressed));
        waitForTutorial = false;
        anim.enabled = true;

        if (StopTutorial != null)
        {
            StopTutorial.Invoke();
        }
    }
}
