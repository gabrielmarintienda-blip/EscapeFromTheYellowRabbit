using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUnlocker : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; // en orden: nivel 1, 2, 3...

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i + 1) <= unlocked;
        }
    }
}