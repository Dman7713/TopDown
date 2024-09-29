using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 10f; // Player's current health
    [SerializeField] private float maxHealth = 10f; // Player's maximum health
    [SerializeField] private Image healthBar; // Reference to the health bar UI

    private void Start()
    {
        health = maxHealth; // Initialize health
        UpdateHealthBar(); // Update health bar at the start
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth; // Update the health bar's fill amount
        }
    }

    // Method to take damage
    public void TakeDamage(float amount)
    {
        health -= amount; // Decrease health by the specified amount
        health = Mathf.Max(health, 0); // Ensure health doesn't go below zero
        UpdateHealthBar(); // Update health bar

        Debug.Log($"Current Health: {health}"); // Log current health

        // Check if health falls below or equal to zero
        if (health <= 0)
        {
            Die(); // Call the die function
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Here you could reload the scene or handle death logic if needed
        RestartGame(); // Restart the game instead of loading a level
    }

    private void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Heal(float amount)
    {
        health += amount; // Increase health
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health to max value
        UpdateHealthBar(); // Update health bar
    }
}
