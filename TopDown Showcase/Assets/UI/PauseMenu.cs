using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI panel
    private bool isPaused = false;

    [SerializeField]
    private GameObject player; // Reference to the player GameObject

    void Update()
    {
        // Check if the game is paused (time scale is 0)
        if (Time.timeScale == 0f)
            return; // Exit if the game is paused

        // Check for pause input (Escape key or P key)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        SetPlayerScriptsEnabled(true); // Enable player scripts
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        SetPlayerScriptsEnabled(false); // Disable player scripts
        isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Ensure the game is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
    }

    public void Quit()
    {
        Time.timeScale = 1f; // Ensure the game is running
        Application.Quit(); // Quit the application

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }

    public bool IsPaused()
    {
        return isPaused; // Returns the current pause state
    }

    private void SetPlayerScriptsEnabled(bool isEnabled)
    {
        if (player != null) // Check if player reference is set
        {
            // Get all MonoBehaviour components on the player and enable/disable them based on isEnabled
            MonoBehaviour[] playerScripts = player.GetComponents<MonoBehaviour>();
            foreach (var script in playerScripts)
            {
                script.enabled = isEnabled;
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject is not assigned in the PauseMenu script.");
        }
    }
}
