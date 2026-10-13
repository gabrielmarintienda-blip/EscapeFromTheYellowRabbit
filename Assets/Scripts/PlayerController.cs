using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float boostSpeed = 35f;
    [SerializeField] private ParticleSystem snowEffect; //Particle effect to play when player cross finish line
    [SerializeField] private ScoreManager scoreManager; //Score manager to update the player's score

    public float speed = 10f; // Base speed of the player
    public float jumpForce = 5f; // Force applied to the player when jumping
    public float raycastDistance = 1f; // Distance for the raycast to check if the player is grounded
    public LayerMask groundLayer; // Layer mask to identify the ground layer for raycasting
    public LayerMask objectLayer;
    private bool isGrounded; // Flag to check if the player is grounded
    SurfaceEffector2D surfaceEffector2D;
    Rigidbody2D rb;
    public Animator animator;
    
    float baseSpeed;
    InputAction moveAction;
    Vector2 moveInput;
  
    float previousRotation;
    float totalRotation;
    int flipCount; 
    int activatePowerUpCount; // Counter to track the number of power-ups activated

        private bool canControllPlayer = true; // Flag to control player input


    public bool CanControllPlayer { get => canControllPlayer; set => canControllPlayer = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        transform.GetChild(0).GetChild(PlayerPrefs.GetInt("SelectedCharacter", 0)).gameObject.SetActive(true); // Activate the selected character based on the saved index
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        surfaceEffector2D = FindAnyObjectByType<SurfaceEffector2D>();
        baseSpeed = surfaceEffector2D.speed; // Store the initial speed of the SurfaceEffector2D
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!canControllPlayer) return; // If player input is disabled, exit the Update method
        PlayerTorque();   
        BoostPlayer();
        CalculateFlips();
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
    /// Calculates the number of flips the player has made based on their rotation.
    /// </summary>
    private void CalculateFlips()
    {
        float currentRotation = transform.rotation.eulerAngles.z;// Get the current rotation of the player in degrees
        totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation); // Calculate the change in rotation since the last frame    
        if(Math.Abs(totalRotation) > 340) // Check if the total rotation has reached a full flip
        {
            flipCount++; // Increment the flip count
            scoreManager.AddScore(flipCount * 100); // Update the score in the ScoreManager
            totalRotation = 0; // Reset the total rotation for the next flip

        }
        previousRotation = currentRotation; // Update the previous rotation for the next frame
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
    void BoostPlayer()
    {
        //Increases the player's speed when the player presses the up arrow key or W key.
        //surface efector speed is increased to booste
       if (moveInput.y > 0)
        {
            surfaceEffector2D.speed = boostSpeed;
          //  snowEffect.Play();


        }
        else
        {
            surfaceEffector2D.speed = baseSpeed;
          //  snowEffect.Stop();
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
    void OnDrawGizmos()
    {
        // Draw a ray in the Scene view to visualize the raycast for checking if the player is grounded
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
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
