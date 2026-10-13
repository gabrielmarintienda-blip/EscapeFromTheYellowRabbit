using UnityEngine;

public class GFreddyAperance : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform player;                 // The player's transform, used to measure the distance
    [SerializeField] private float activationDistance = 15f;   // Distance at which the character appears

    [Header("Character")]
    [SerializeField] private GameObject visual;                // Child object that holds the character's sprite (hidden at start)
    [SerializeField] private bool hideWhenPlayerLeaves = true; // If true, the character hides again when the player moves away

    [Header("Sound")]
    [SerializeField] private AudioSource laughSound;           // Laugh played when the character appears (keep it on THIS object, not on the hidden child)

    private bool isVisible = false; // Current state, so the laugh only plays once per appearance

    private void Start()
    {
        if (player == null)
            Debug.LogWarning("GFreddyAperance: assign the Player in the Inspector.", this);

        // The character starts hidden
        if (visual != null)
            visual.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        // Distance between this character and the player (2D)
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= activationDistance)
            Appear();
        else if (hideWhenPlayerLeaves)
            Disappear();
    }

    private void Appear()
    {
        if (isVisible) return; // Already visible, do nothing
        isVisible = true;

        // Show the character
        if (visual != null)
            visual.SetActive(true);

        // Play the laugh
        if (laughSound != null)
            laughSound.Play();
    }

    private void Disappear()
    {
        if (!isVisible) return; // Already hidden, do nothing
        isVisible = false;

        // Hide the character
        if (visual != null)
            visual.SetActive(false);
    }
}