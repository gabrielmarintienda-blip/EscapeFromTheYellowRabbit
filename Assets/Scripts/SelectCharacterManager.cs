using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacterManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Level1"; // Name of the game scene (must be in Build Settings)

    // Called by each character button. The index is set in the button's OnClick event
    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetInt("SelectedCharacter", index); // Save the chosen character
        PlayerPrefs.Save();
    }

    public void SelectCharacterAndGoMenu(int index)
    {
        PlayerPrefs.SetInt("SelectedCharacter", index); // Save the chosen character
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu"); // Return to the menu scene

    }

    // Use this if you want to pick a character and start the game with a single click
    public void SelectAndPlay(int index)
    {
        SelectCharacter(index);
        SceneManager.LoadScene(gameSceneName);
    }

    // Use this for a separate "Play" button after choosing a character
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void CharactersBackToMenu()
    {
        SceneManager.LoadScene("Menu"); // Return to the menu scene
    }
}