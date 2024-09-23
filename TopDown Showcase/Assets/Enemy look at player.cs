using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer2D : MonoBehaviour
{
    // Reference to the player
    private Transform playerTransform;

    void Start()
    {
        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure there is a GameObject with the 'Player' tag in the scene.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction from the enemy to the player
            Vector3 direction = playerTransform.position - transform.position;

            // Get the angle to rotate towards the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation to the enemy, adjust by adding 180 degrees
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + -90));
        }
    }
}