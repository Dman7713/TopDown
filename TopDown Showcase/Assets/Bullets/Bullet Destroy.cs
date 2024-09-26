using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    public GameObject destructionEffectPrefab; // Prefab for the destruction effect
    public float destroyDelay = 0.5f; // Time to wait before destroying the destruction effect

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object the bullet collides with has the tag "Wall" or "Enemy"
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            // Instantiate the destruction effect prefab at the bullet's position and rotation
            if (destructionEffectPrefab != null)
            {
                GameObject effect = Instantiate(destructionEffectPrefab, transform.position, transform.rotation);
                Destroy(effect, destroyDelay); // Destroy the effect after the specified delay
            }

            // Immediately destroy the bullet
            Destroy(gameObject);
        }
    }
}