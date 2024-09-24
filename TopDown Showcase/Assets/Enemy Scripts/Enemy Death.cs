using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Number of bullets required to destroy this object
    public int hitPoints = 1;

    // Reference to the coin prefab
    public GameObject coinPrefab;

    // The position offset where the coin will be spawned
    public Vector3 coinSpawnOffset = new Vector3(0, 0, 0);

    // Reference to the particle system prefab
    public GameObject deathEffectPrefab; // Assign the particle system prefab in the Inspector

    // Duration for which the death effect plays
    public float deathEffectDuration = 2f; // Default duration

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with has a tag "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Reduce hitPoints by 1
            hitPoints--;

            // Check if hitPoints has reached 0 or below
            if (hitPoints <= 0)
            {
                // Spawn the coin
                SpawnCoin();

                // Play death effect
                PlayDeathEffect();

                // Destroy this game object
                Destroy(gameObject);
            }
        }
    }

    private void SpawnCoin()
    {
        if (coinPrefab != null)
        {
            // Calculate the spawn position
            Vector3 spawnPosition = transform.position + coinSpawnOffset;

            // Instantiate the coin prefab at the calculated position
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Coin Prefab is not assigned in the Inspector.");
        }
    }

    private void PlayDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            // Instantiate the particle system prefab at the enemy's position
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

            // Optionally, destroy the particle effect after it has finished playing
            Destroy(deathEffect, deathEffectDuration); // Use the adjustable duration
        }
        else
        {
            Debug.LogWarning("Death Effect Prefab is not assigned in the Inspector.");
        }
    }
}