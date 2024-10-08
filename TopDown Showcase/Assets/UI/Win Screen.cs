using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public GameObject winScreenUI; // The UI element for the win screen
    public GameObject targetEnemy; // The enemy to track (set from the spawner)

    private void Start()
    {
        // Ensure the win screen is hidden at the start
        winScreenUI.SetActive(false);
    }

    private void Update()
    {
        // Check if the target enemy has been defeated
        if (targetEnemy == null && !winScreenUI.activeSelf)
        {
            ActivateWinScreen();
        }
    }

    // Method to activate the win screen
    public void ActivateWinScreen()
    {
        winScreenUI.SetActive(true); // Show the win screen UI
        Time.timeScale = 0; // Freeze the game when the win screen is active
    }

    // Method to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Method to quit the game
    public void QuitGame()
    {
        Time.timeScale = 1; // Ensure the time scale is reset
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }
}
