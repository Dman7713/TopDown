using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class WinScreenManager : MonoBehaviour
{
    public GameObject winScreenUI; // The UI element for the win screen
    public Timer timerScript; // Reference to the Timer script that controls the countdown

    private void Start()
    {
        // Ensure the win screen is hidden at the start
        winScreenUI.SetActive(false);
    }

    private void Update()
    {
        // Check if the timer has reached zero
        if (timerScript != null && timerScript.currentTime <= 0)
        {
            ActivateWinScreen();
        }
    }

    // Function to activate the win screen UI
    private void ActivateWinScreen()
    {
        winScreenUI.SetActive(true); // Show the win screen UI
        Time.timeScale = 0; // Freeze the game time when the win screen is active
    }

    // Method to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Method to quit the game
    public void QuitGame()
    {
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }
}
