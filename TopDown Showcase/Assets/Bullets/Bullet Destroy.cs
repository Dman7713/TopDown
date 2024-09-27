using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    public GameObject destructionEffectPrefab; // Prefab for the destruction effect
    public float destroyDelay = 0.5f; // Time to wait before destroying the destruction effect

    [SerializeField]
    GameObject bloodPrefab; // Blood prefab to be placed when the bullet is destroyed
    [SerializeField]
    float bloodDuration = 1f; // Duration for how long the blood prefab should stay

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object the bullet collides with has the tag "Wall", "Enemy", or "Player"
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            // Instantiate the destruction effect prefab at the bullet's position and rotation
            if (destructionEffectPrefab != null)
            {
                GameObject effect = Instantiate(destructionEffectPrefab, transform.position, transform.rotation);
                Destroy(effect, destroyDelay); // Destroy the effect after the specified delay
            }

            // Instantiate the blood prefab at the bullet's position
            if (bloodPrefab != null)
            {
                GameObject bloodInstance = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
                Destroy(bloodInstance, bloodDuration); // Destroy the blood prefab after the specified duration
            }

            // Immediately destroy the bullet
            Destroy(gameObject);
        }
    }
}
