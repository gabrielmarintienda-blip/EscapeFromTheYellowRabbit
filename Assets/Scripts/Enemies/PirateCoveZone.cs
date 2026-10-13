using UnityEngine;

public class PirateCoveZone : MonoBehaviour
{
    [SerializeField] private PirateCoveController controller; // The cove controller
    [SerializeField] private int stageToActivate;             // Stage to jump to when the player enters

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the player
        if (other.GetComponentInParent<PlayerController>() == null) return;

        controller.GoToStage(stageToActivate);
    }
}