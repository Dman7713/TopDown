using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerHealth playerHealth; // Reference to the PlayerHealth component
    private CameraShake cameraShake; // Reference to the CameraShake component

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>(); // Get the PlayerHealth component on the player
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Get the CameraShake component from the main camera

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found on player.");
        }

        if (cameraShake == null)
        {
            Debug.LogError("CameraShake component not found on the main camera.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1); // Take 1 damage
            cameraShake.ShakeCamera(); // Shake the camera
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Continuously take damage while in contact with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1 * Time.deltaTime); // Take damage over time
            cameraShake.ShakeCamera(); // Shake the camera
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for health pickups
        if (collision.CompareTag("HealthPickup"))
        {
            playerHealth.Heal(20); // Heal the player (example value)
            Destroy(collision.gameObject); // Destroy the health pickup after collecting
        }
    }
}
