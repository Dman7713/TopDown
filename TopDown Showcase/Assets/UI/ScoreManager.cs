using UnityEngine;
using TMPro; // Required for TextMesh Pro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the current score text
    public TextMeshProUGUI highScoreText; // Reference to the high score text

    private int currentScore = 0; // Track the player's current score
    private int highScore = 0; // Track the high score

    private void Start()
    {
        // Load the high score from PlayerPrefs (default to 0 if not set)
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI(); // Update the UI with the initial score and high score
    }

    // Call this method to add points to the score
    public void AddScore(int points)
    {
        currentScore += points; // Add points to the current score
        UpdateScoreUI(); // Update the UI

        // Check if the current score is higher than the high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore); // Save the new high score
            PlayerPrefs.Save(); // Ensure the high score is saved to disk
        }
    }

    // Update the UI to show the current score and high score
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore;
        highScoreText.text = "High Score: " + highScore;
    }

    // Optionally, call this method to reset the score and high score
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScore = 0;
        UpdateScoreUI();
    }
}
