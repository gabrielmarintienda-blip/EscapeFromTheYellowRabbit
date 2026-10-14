using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private float crashDelay = 1f;
    [SerializeField] private ParticleSystem bloodEffect; // Plays when the player crashes

    PlayerController playerController;
    GameFlowManager gameFlow;
    bool hasCrashed = false; // Only the first crash counts

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        gameFlow = FindAnyObjectByType<GameFlowManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCrashed) return;

        int layer = other.gameObject.layer;

        // Crash against the floor: normal death
        if (layer == LayerMask.NameToLayer("Floor"))
        {
            NormalDeath();
        }
        // Touched an enemy: jumpscare
        else if (layer == LayerMask.NameToLayer("Enemies"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                hasCrashed = true;
                playerController.CanControllPlayer = false; // Disable player input
                enemy.TriggerJumpscare(); // This enemy's own image and sound
            }
            else
            {
                NormalDeath(); // Enemy object without an Enemy script
            }
        }
    }

    void NormalDeath()
    {
        hasCrashed = true;
        Debug.Log("Player has crashed!");
        playerController.CanControllPlayer = false;
        bloodEffect.Play();
        gameFlow.PlayDeathAudio(); // Stop the music and play the death sound now
        Invoke(nameof(ShowDeath), crashDelay);
    }

    void ShowDeath()
    {
        gameFlow.ShowDeathScreen();
        
    }
}