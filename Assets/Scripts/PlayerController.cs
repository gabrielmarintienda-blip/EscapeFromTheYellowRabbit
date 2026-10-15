using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float boostSpeed = 35f;
    [SerializeField] private ParticleSystem snowEffect; //Particle effect to play when player cross finish line
    [SerializeField] private ScoreManager scoreManager; //Score manager to update the player's score
    [SerializeField] private TextMeshProUGUI clockText;     // Text that shows the in-game time
    [SerializeField] private GameObject sixAmAnimation;     // UI object with the "6 AM" animation

    public float speed = 10f; // Base speed of the player
    public float jumpForce = 5f; // Force applied to the player when jumping
    public float raycastDistance = 1f; // Distance for the raycast to check if the player is grounded
    public LayerMask groundLayer; // Layer mask to identify the ground layer for raycasting
    public LayerMask objectLayer;
    private bool isGrounded; // Flag to check if the player is grounded
    SurfaceEffector2D surfaceEffector2D;
    Rigidbody2D rb;
    public Animator animator;
    [SerializeField] private GameFlowManager gameFlow;

    private float aliveTime = 0f;          // Total time alive (seconds)
    private int secondsAwarded = 0;        // Seconds already scored
    private int bonusesAwarded = 0;        // 30-second bonuses already given
    private const int MaxSeconds = 180;    // 3 minutes
    private const int PointsPerSecond = 100;
    private const int BonusEvery = 30;
    private const int BonusPoints = 500;
    
    float baseSpeed;
    InputAction moveAction;
    Vector2 moveInput;
  
    float previousRotation;
    float totalRotation;
    int flipCount; 
    int activatePowerUpCount; // Counter to track the number of power-ups activated

        private bool canControllPlayer = true; // Flag to control player input
        private const int CharacterCount = 5; // Number of selectable characters


    public bool CanControllPlayer { get => canControllPlayer; set => canControllPlayer = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Assign the input and effector first, so a problem with the animator can't break them
        moveAction = InputSystem.actions.FindAction("Move");
        surfaceEffector2D = FindAnyObjectByType<SurfaceEffector2D>();
        baseSpeed = surfaceEffector2D.speed;

        SetupCharacterAnimator();
        UpdateClock();
    }

    // Applies the animator override of the character chosen in the selection screen
    // Plays the walk animation of the character chosen in the selection screen
    private void SetupCharacterAnimator()
    {
        animator = GetComponentInChildren<Animator>(); // The Animator lives on the "Visual" child

        if (animator == null)
        {
            Debug.LogError("PlayerController: no Animator found in the children of the Player.", this);
            return;
        }

        // Read the saved character (0 if none was saved) and keep it inside the valid range
        int selected = Mathf.Clamp(PlayerPrefs.GetInt("SelectedCharacter", 0), 0, CharacterCount - 1);

        // State names are WalkCharacter1 ... WalkCharacter5, so we add 1 to the index
        string stateName = "WalkCharacter" + (selected + 1);
        Debug.Log("Selected character: " + selected + " -> playing " + stateName);

        animator.Play(stateName); // Jump straight to the walk state of that character
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!canControllPlayer) return; // If player input is disabled, exit the Update method
        CalculateTime();
        PlayerTorque();   
        PlayerJump();
        
    }


    private void PlayerJump()
    {
        // The keyboard can be null (e.g. no keyboard connected), so we check first
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Combine both layers: the player is grounded on the floor OR on objects
        LayerMask groundMask = groundLayer | objectLayer;

        // Cast a ray downwards against the combined mask
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundMask);
        isGrounded = hit.collider != null;

        // Debug line in the Scene view: green = grounded, red = in the air
        Debug.DrawRay(transform.position, Vector2.down * raycastDistance, isGrounded ? Color.green : Color.red);

        // Jump only when grounded and the space key was pressed this frame
        if (isGrounded && keyboard.spaceKey.wasPressedThisFrame)
        {
            // Apply an instant upward force to the player
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Adds score for time alive: 100 per second and 500 extra every 30 seconds (max 3 minutes).
    /// Call from Update() while the player is alive.
    /// </summary>
    private void CalculateTime()
    {
        if (secondsAwarded >= MaxSeconds) return; // Max time already reached

        aliveTime += Time.deltaTime; // Accumulate the time elapsed since the last frame

        // Full seconds elapsed (capped at the maximum)
        int currentSeconds = Mathf.Min((int)aliveTime, MaxSeconds);

        // Award points for each newly completed second
        while (secondsAwarded < currentSeconds)
        {
            secondsAwarded++;
            scoreManager.AddScore(PointsPerSecond);
            
           // Bonus and clock update every 30 seconds
            if (secondsAwarded % BonusEvery == 0)
            {
                bonusesAwarded++;
                scoreManager.AddScore(BonusPoints);
                UpdateClock(); // Advance the in-game hour

                if (secondsAwarded >= MaxSeconds)
                {
                    FinishNight(); // 6 AM reached
                }
            }
        }
    }

    /// <summary>
    /// Updates the clock text: 12 AM, 1 AM, 2 AM ... 6 AM (one hour every 30 seconds).
    /// </summary>
    private void UpdateClock()
    {
        int hoursPassed = secondsAwarded / BonusEvery; // 0 to 6
        int displayHour = hoursPassed == 0 ? 12 : hoursPassed; // 0 is shown as 12
        clockText.text = displayHour + "AM";
    }

    /// <summary>
    /// Stops the game and plays the 6 AM animation.
    /// </summary>
    private void FinishNight()
    {
        canControllPlayer = false;   // Stop player input
        surfaceEffector2D.speed = 0f;    // Stop the moving surface
        rb.simulated = false;            // Freeze the player's physics
        sixAmAnimation.SetActive(true);  // Start the 6 AM animation     
        gameFlow.ShowNightComplete(); // Fade in the 6 AM image, then show the menu
    }


    /// <summary>
    /// Applies torque to the player based on the horizontal input from the Move action.
    /// </summary>
    void PlayerTorque()
    {
        moveInput = moveAction.ReadValue<Vector2>(); 
        if (moveInput.x < 0)
        {
            rb.AddTorque(torqueAmount);
        }
        else if (moveInput.x > 0)
        {
            rb.AddTorque(-torqueAmount);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        int LayerIndex = LayerMask.NameToLayer("Floor");
        if(collision.gameObject.layer == LayerIndex)
        {
            Debug.Log("Player is on the floor!");
            snowEffect.Play();
    }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        int LayerIndex = LayerMask.NameToLayer("Floor");
        if(collision.gameObject.layer == LayerIndex)
        {
            Debug.Log("Player is on the floor!");
            snowEffect.Stop();
    }
    }


    internal void ApplyPowerUp(PowerUpScriptableObject powerUpData)
    {
        activatePowerUpCount++; // Increment the power-up activation count
        if(powerUpData.PowerUpType == "Speed")
        {
            baseSpeed += powerUpData.PowerUpValue; // Increase the base speed by the power-up value
            boostSpeed += powerUpData.PowerUpValue; // Increase the boost speed by the power-up value
        }
    }

    public void DeactivatePowerUp(PowerUpScriptableObject powerUpData)
    {
        activatePowerUpCount--; // Decrement the power-up activation count
        if (activatePowerUpCount == 0) // If no power-ups are active, reset speeds to base values
        {
            if(powerUpData.PowerUpType == "Speed")
            {
                baseSpeed -= powerUpData.PowerUpValue; // Decrease the base speed by the power-up value
                boostSpeed -= powerUpData.PowerUpValue; // Decrease the boost speed by the power-up value
            }
        }
    }


}
