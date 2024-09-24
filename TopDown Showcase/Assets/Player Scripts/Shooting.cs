using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab assigned in the Inspector
    public GameObject fireEffectPrefab; // The effect prefab assigned in the Inspector
    public Transform shootingPoint; // The point from which bullets are shot (assigned in Inspector)
    public float fireRate = 0.2f; // Time between consecutive shots
    public float bulletSpeed = 20f; // Speed at which the bullet moves
    public float spreadAngle = 5f; // Maximum angle for bullet spread
    public float fireEffectDuration = 1f; // Duration of the fire effect

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
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;  // Ensure the z-coordinate is zero for 2D

            // Calculate the shooting direction towards the mouse position
            Vector2 shootDirection = (mousePosition - shootingPoint.position).normalized;

            // Apply random spread to the shooting direction
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            shootDirection = Quaternion.Euler(0, 0, randomSpread) * shootDirection;

            // Set the velocity of the bullet to move in the calculated direction
            rb.velocity = shootDirection * bulletSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the bullet prefab.");
        }

        // Instantiate the fire effect at the shooting point and set it as a child
        if (fireEffectPrefab != null)
        {
            GameObject fireEffect = Instantiate(fireEffectPrefab, shootingPoint.position, shootingPoint.rotation, shootingPoint);
            StartCoroutine(DestroyFireEffectAfterDuration(fireEffect, fireEffectDuration));
        }
    }

    private IEnumerator DestroyFireEffectAfterDuration(GameObject fireEffect, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(fireEffect);
    }
}