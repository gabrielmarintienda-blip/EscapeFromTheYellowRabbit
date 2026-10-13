using UnityEngine;

public class PowerupManage : MonoBehaviour
{
  [SerializeField] private PowerUpScriptableObject powerUpData; // Reference to the PowerUpScriptableObject containing the power-up data

    PlayerController playerController; // Reference to the PlayerController script to modify player behavior
    
    SpriteRenderer powerUpspriteRenderer; // Reference to the SpriteRenderer component to change the power-up's appearance

    float timeLeft;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>(); // Find the PlayerController in the scene
        powerUpspriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Get the SpriteRenderer component attached to this GameObject
        timeLeft = powerUpData.TimeLimit; // Initialize the time left for the power-up effect
    }

    private void Update()
    {
       CountDownUpTime(); // Call the method to count down the time left for the power-up effect
    }

    //<summary>
    /// Counts down the time left for the power-up effect and disables the power-up when time  
    /// </summary>
    
    private void CountDownUpTime()
    {
        if (powerUpspriteRenderer.enabled ==false)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime; // Decrease the time left by the time elapsed since the last frame
            }
            else
            {
               playerController.DeactivatePowerUp(powerUpData); // Call the method to deactivate the power-up effect on the player
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && powerUpspriteRenderer.enabled) // Check if the colliding object is the player
        {

            powerUpspriteRenderer.enabled = false; // Disable the power-up's sprite renderer to make it invisible
            playerController.ApplyPowerUp(powerUpData); // Apply the power-up effect to the player
        }
    }
    
}
