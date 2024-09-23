using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab assigned in the Inspector
    public Transform shootingPoint; // The point from which bullets are shot (assigned in Inspector)
    public float fireRate = 0.2f; // Time between consecutive shots
    public float bulletSpeed = 20f; // Speed at which the bullet moves
    public float spreadAngle = 5f; // Maximum angle for bullet spread

    private float nextFireTime = 0f; // Time at which the next shot can be fired

    private void Update()
    {
        // Check if left mouse button (or another input key) is pressed and the gun can fire
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        // Instantiate the bullet at the shooting point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Get the Rigidbody2D component from the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculate a random spread angle within the specified range
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);

            // Apply the random spread to the shooting direction by rotating the shooting point's direction
            Vector2 shootDirection = Quaternion.Euler(0, 0, randomSpread) * shootingPoint.up;

            // Set the velocity of the bullet to move in the spread direction
            rb.velocity = shootDirection * bulletSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the bullet prefab.");
        }
    }
}