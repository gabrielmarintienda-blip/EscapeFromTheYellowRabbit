using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  
    public void PlayStartGame(int levels)
    {
        SceneManager.LoadScene($"Level{levels}"); // Load the game scene when the "Start Game" button is clicked
    }

    public void StartGameMenu()
    {
        SceneManager.LoadScene("SelectLevels"); // Load the game scene when the "Start Game" button is clicked
    }


    public void goToCredits()
    {
        SceneManager.LoadScene("Credits"); // Load the game scene when the "Return to Menu" button is clicked
    }

    public void goToSelectCharacter()
    {
        SceneManager.LoadScene("SelectCharacters"); // Load the game scene when the "Return to Menu" button is clicked
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application when the "Quit" button is clicked
    }

    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex); // Save the selected character index to PlayerPrefs
        PlayerPrefs.Save(); // Save the PlayerPrefs data
        SceneManager.LoadScene("Menu"); // Load the game scene after selecting a character
    }
}
