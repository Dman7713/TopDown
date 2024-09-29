using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelOnCollision : MonoBehaviour
{
    public float repelForce = 5f; // The force applied to repel the player
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with has the "Wall" tag
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Calculate the direction away from the wall
            Vector2 repelDirection = (transform.position - collision.transform.position).normalized;

            // Apply the repel force
            rb.AddForce(repelDirection * repelForce, ForceMode2D.Impulse);
        }
    }
}