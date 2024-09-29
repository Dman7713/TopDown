using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For UI components

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startMenuUI; // Reference to the start menu UI

    private void Start()
    {
        // Pause the game at the start
        Time.timeScale = 0f; // Set time scale to 0 to pause the game
        startMenuUI.SetActive(true); // Ensure the start menu UI is active
    }

    // Method to start the game
    public void StartGame()
    {
        // Unpause the game
        Time.timeScale = 1f; // Set time scale back to normal

        // Disable the start menu UI
        startMenuUI.SetActive(false);

        // Load the first scene (make sure to set the correct index)
        SceneManager.LoadScene(1); // Adjust this index based on your build settings
    }

    // Method to quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#else
        Application.Quit(); // Quit the application
#endif
    }
}
