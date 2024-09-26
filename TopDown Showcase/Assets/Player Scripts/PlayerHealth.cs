using System.Collections;
using System.Collections.Generic;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        // Check if the player hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); // Take 1 damage
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Continuously take damage while in contact with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1 * Time.deltaTime); // Take damage over time
        }
    }

    private void TakeDamage(float amount)
    {
        health -= amount; // Decrease health by the specified amount
        UpdateHealthBar(); // Update health bar

        // Check if health falls below or equal to zero
        if (health <= 0)
        {
            Die(); // Call the die function
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        RestartGame(); // Restart the game instead of loading a level
    }

    private void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for health pickups
        if (collision.CompareTag("HealthPickup"))
        {
            Heal(20); // Heal the player (example value)
            Destroy(collision.gameObject); // Destroy the health pickup after collecting
        }
    }

    public void Heal(float amount)
    {
        health += amount; // Increase health
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health to max value
        UpdateHealthBar(); // Update health bar
    }
}
