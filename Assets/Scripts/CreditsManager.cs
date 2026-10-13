using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
      public void CreditsBackToMenu()
    {
        SceneManager.LoadScene("Menu"); // Load the game scene when the "Return to Menu" button is clicked
    }

}
