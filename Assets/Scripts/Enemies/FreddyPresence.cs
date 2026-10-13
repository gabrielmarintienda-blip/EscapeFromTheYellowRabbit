using UnityEngine;

public class FreddyPresence : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform player;
    [SerializeField] private float activationDistance = 15f;

    [Header("Efecto HUD")]
    [SerializeField] private GameObject darknessEffect;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;

    private bool isActive = false;

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Freddy está cerca
        if (distance <= activationDistance)
        {
            ActivateEffect();
        }
        // Freddy está lejos
        else
        {
            DeactivateEffect();
        }
    }

    private void ActivateEffect()
    {
        if (isActive)
            return;

        isActive = true;

        // Activar oscuridad
        if (darknessEffect != null)
            darknessEffect.SetActive(true);

        // Reproducir sonido
        if (audioSource != null)
            audioSource.Play();
    }

    private void DeactivateEffect()
    {
        if (!isActive)
            return;

        isActive = false;

        // Desactivar oscuridad
        if (darknessEffect != null)
            darknessEffect.SetActive(false);
    }
}