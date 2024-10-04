using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 10f; // Player's current health
    [SerializeField] private float maxHealth = 10f; // Player's maximum health
    [SerializeField] private Image healthBar; // Reference to the health bar UI
    [SerializeField] private AudioSource damageAudioSource; // AudioSource assigned in the Inspector

    private bool isTouchingEnemy = false; // Tracks if player is currently touching an enemy

    private void Start()
    {
        health = maxHealth; // Initialize health
        UpdateHealthBar(); // Update health bar at the start

        // Ensure the audio source is assigned in the Inspector
        if (damageAudioSource == null)
        {
            Debug.LogError("AudioSource for damage sound is not assigned!");
        }
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
        if (health > 0)
        {
            health -= amount; // Decrease health by the specified amount
            health = Mathf.Max(health, 0); // Ensure health doesn't go below zero
            UpdateHealthBar(); // Update health bar

            Debug.Log($"Current Health: {health}"); // Log current health

            // Play the damage sound if not already playing
            if (damageAudioSource != null && !damageAudioSource.isPlaying)
            {
                damageAudioSource.Play(); // Start playing the damage sound
            }

            // Check if health falls below or equal to zero
            if (health <= 0)
            {
                Die(); // Call the die function
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        StopDamageSound(); // Stop the damage sound when player dies
        RestartGame(); // Restart the game
    }

    private void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StopDamageSound()
    {
        if (damageAudioSource != null && damageAudioSource.isPlaying)
        {
            damageAudioSource.Stop(); // Stop playing the damage sound
        }
    }

    // Heal method to restore health
    public void Heal(float amount)
    {
        if (health < maxHealth) // Only heal if health is below max
        {
            health += amount; // Increase health
            health = Mathf.Clamp(health, 0, maxHealth); // Clamp health to max value
            UpdateHealthBar(); // Update health bar

            // Stop the sound when the player is fully healed or no longer taking damage
            if (health == maxHealth)
            {
                StopDamageSound();
            }
        }
    }

    // Detect when player collides with an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isTouchingEnemy = true; // Player is touching the enemy
            TakeDamage(1f); // Apply immediate damage
        }
    }

    // Detect when player exits the collision with the enemy
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isTouchingEnemy = false; // Player is no longer touching the enemy
            StopDamageSound(); // Stop the damage sound
        }
    }

    private void Update()
    {
        // Continuously apply damage while in contact with the enemy
        if (isTouchingEnemy && health > 0)
        {
            TakeDamage(0.1f * Time.deltaTime); // Continuous damage over time
        }

        // Ensure the sound stops when the player is safe
        if (!isTouchingEnemy)
        {
            StopDamageSound(); // Stop the damage sound when safe
        }
    }
}
