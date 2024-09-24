using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object the bullet collides with has the tag "Enemy", "Wall", or "Player"
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("Player"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}