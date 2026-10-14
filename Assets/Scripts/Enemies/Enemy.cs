using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Jumpscare")]
    [SerializeField] private Sprite jumpscareSprite;   // Image shown when this enemy catches the player
    [SerializeField] private AudioClip jumpscareSound; // Sound played with the image
    [SerializeField] private bool shake = true;        // Shake the image or keep it still

    private GameFlowManager gameFlow;

    void Start()
    {
        gameFlow = FindAnyObjectByType<GameFlowManager>();
    }

    /// <summary>
    /// Triggers this enemy's jumpscare. Called by CrashDetector or by a solid collision.
    /// </summary>
    public void TriggerJumpscare()
    {
        gameFlow.PlayJumpscare(jumpscareSprite, jumpscareSound, shake);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            TriggerJumpscare();
        }
    }
}