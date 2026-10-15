using UnityEngine;
using UnityEngine.SceneManagement;

public class BonnieBallpitAttack : MonoBehaviour
{
     
   //  [SerializeField] private float reloadDelay = 1.5f; // Time before restarting the level
    [Header("Jumpscare")]
    [SerializeField] private Sprite jumpscareSprite;   // Image shown when this enemy catches the player
    [SerializeField] private AudioClip jumpscareSound; // Sound played with the image
    [SerializeField] private bool shake = true;        // Shake the image or keep it still

    private bool killed; // Avoids triggering the death more than once

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (killed) return;

        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player == null) return;

        killed = true;
        player.CanControllPlayer = false; // Disable player input
        Debug.Log("Player was grabbed by the arm!");

        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            TriggerJumpscare();
        }
    }

}
