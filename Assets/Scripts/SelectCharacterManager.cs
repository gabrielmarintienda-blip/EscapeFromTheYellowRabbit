using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectCharacterManager : MonoBehaviour
{
     public void CharactersBackToMenu()
    {
        SceneManager.LoadScene("Menu"); // Load the game scene when the "Return to Menu" button is clicked
    }
}
