using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText; // Text that displays the player's score

    private int totalScore = 0; // Accumulated score

    public void AddScore(int score)
    {
        totalScore += score; // Add the new points to the total
        scoreText.text = totalScore.ToString("00000"); // Update the score text
    }
}