using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Get the highest unlocked level from PlayerPrefs, default is 1
        Transform levels = transform.GetChild(0); // Get the first child of the current GameObject, which contains the level buttons
        for (int i = 0; i < levels.childCount; i++)
        {
            Button levelButton = levels.GetChild(i).GetComponent<Button>(); // Get the Button component of the current level button
            levelButton.interactable = (i < unlockedLevel); // Enable the button if the level is unlocked, otherwise disable it
        }
    }


}
