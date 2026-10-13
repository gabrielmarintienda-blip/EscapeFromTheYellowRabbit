using UnityEngine;
using UnityEngine.SceneManagement;

public class BonnieBallpitAttack : MonoBehaviour
{
     [SerializeField] private float reloadDelay = 1.5f; // Time before restarting the level

    private bool killed; // Avoids triggering the death more than once

    void OnTriggerEnter2D(Collider2D other)
    {
        if (killed) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;

        killed = true;
        player.CanControllPlayer = false; // Disable player input
        Debug.Log("Player was grabbed by the arm!");

        Invoke(nameof(RestartLevel), reloadDelay);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
