using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBar; // Reference to the health bar Image
    private float maxHealth; // Maximum health

    // Initialize the health bar
    public void Initialize(float maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateHealthBar(maxHealth); // Set initial health bar value
    }

    // Update the health bar based on current health
    public void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth; // Update health bar
        }
        else
        {
            Debug.LogError("Health bar not assigned!");
        }
    }
}
