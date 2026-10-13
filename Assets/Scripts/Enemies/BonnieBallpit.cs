using UnityEngine;

public class BonnieBallpit : MonoBehaviour
{
   [SerializeField] private SpriteRenderer normalSprite; // Normal ball pit sprite (auto-filled if left empty)
    [SerializeField] private GameObject trapVisual;       // Whole TrapPit object (sprite + arm hitbox)
    [SerializeField] private float delay = 0f;            // Optional delay before the swap
    [SerializeField] private AudioSource scareSound;      // Optional sound

    private bool triggered; // Makes sure the trap only activates once

    void Awake()
    {
        // If nothing is assigned, use the SpriteRenderer on this same object
        if (normalSprite == null) normalSprite = GetComponent<SpriteRenderer>();

        if (normalSprite == null || trapVisual == null)
        {
            Debug.LogError("BallPitTrap: assign Normal Sprite and Trap Visual in the Inspector.", this);
            enabled = false; // Stop the script so it doesn't throw more errors
            return;
        }

        // Start state: normal pit visible, trap hidden
        normalSprite.enabled = true;
        trapVisual.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        // Only react to the player (also works if the collider is on a child)
        if (other.GetComponentInParent<PlayerController>() == null) return;

        triggered = true;
        Invoke(nameof(ActivateTrap), delay);
    }

    private void ActivateTrap()
    {
        normalSprite.enabled = false;   // Hide the normal pit
        trapVisual.SetActive(true);     // Show the pit with the arm (and enable its hitbox)

        if (scareSound != null) scareSound.Play();
    }
}
