using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f;
    [SerializeField] private ParticleSystem finishEffect; //Particle effect to play when player cross finish line
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has crossed the finish line!");
            finishEffect.Play();


            Invoke(nameof(ReloadScene), reloadDelay);
        }
    }

    void NextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel + 1); // Unlock the next level
        PlayerPrefs.Save(); // Save the PlayerPrefs to persist the unlocked level
        if(unlockedLevel > 4)
        {
            SceneManager.LoadScene("Menu"); // Load the menu scene if the last level is completed
        }
        else
        {
            SceneManager.LoadScene($"Level_{unlockedLevel + 1}"); // Load the next scene in the build index
        }

        
    }

    void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
