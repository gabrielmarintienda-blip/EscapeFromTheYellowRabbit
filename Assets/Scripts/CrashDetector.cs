using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{       
        [SerializeField] private float crashDelay = 1f;
        [SerializeField] private ParticleSystem bloodEffect; //Particle effect to play when player cross finish line
    PlayerController playerController;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if (other.gameObject.layer == layerIndex)
        {
            Debug.Log("Player has crashed!");
            playerController.CanControllPlayer = false; // Disable player input
            bloodEffect.Play();
            //TODO: You can add additional logic here, such as triggering a game over condition or restarting the level.
            Invoke(nameof(ReloadScene), crashDelay);
        }
        int layerIndex2 = LayerMask.NameToLayer("Enemies");
        if (other.gameObject.layer == layerIndex2)
        {
            Debug.Log("Player has crashed!");
            playerController.CanControllPlayer = false; // Disable player input
            bloodEffect.Play();
            Invoke(nameof(ReloadScene), crashDelay);
        }
    }

    void ReloadScene(){
        //Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
