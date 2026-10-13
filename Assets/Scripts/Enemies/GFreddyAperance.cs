
using UnityEngine;

public class GFreddyAperance : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform player;
    [SerializeField] private float activationDistance = 15f;

    [Header("Freddy")]
    [SerializeField] private SpriteRenderer freddySprite;

    [Header("Efecto de oscuridad")]
    [SerializeField] private GameObject darknessEffect;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;

    private bool isActive = false;

    private void Start()
    {
        // Freddy empieza invisible
        if (freddySprite != null)
            freddySprite.enabled = false;

        // Oscuridad empieza desactivada
        if (darknessEffect != null)
            darknessEffect.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= activationDistance)
        {
            Appear();
        }
        else
        {
            Disappear();
        }
    }

    private void Appear()
    {
        if (isActive)
            return;

        isActive = true;

        // Mostrar a Freddy
        if (freddySprite != null)
            freddySprite.enabled = true;

        // Activar oscuridad
        if (darknessEffect != null)
            darknessEffect.SetActive(true);

        // Reproducir sonido
        if (audioSource != null)
            audioSource.Play();
    }

    private void Disappear()
    {
        if (!isActive)
            return;

        isActive = false;

        // Ocultar a Freddy
        if (freddySprite != null)
            freddySprite.enabled = false;

        // Desactivar oscuridad
        if (darknessEffect != null)
            darknessEffect.SetActive(false);
    }
}