using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab assigned in the Inspector
    public Transform shootingPoint; // The point from which bullets are shot (assigned in Inspector)
    public float fireRate = 0.2f; // Time between consecutive shots
    public float bulletSpeed = 20f; // Speed at which the bullet moves
    public SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer
    public Sprite idleSprite; // The player's idle or walking sprite
    public Sprite shootingSprite; // The sprite to show when the player is shooting
    public float spriteSwitchInterval = 0.1f; // Time to switch between sprites

    private float nextFireTime = 0f; // Time at which the next shot can be fired
    private bool isFiring = false; // Is the player holding down the fire button?
    private bool isSwitchingSprites = false; // Is the sprite switching coroutine running?

    private void Update()
    {
        // Check if the left mouse button is held down and the gun can fire
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;

            // Start switching between the sprites if not already doing so
            if (!isSwitchingSprites)
            {
                StartCoroutine(SwitchSpritesBackAndForth());
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            // Stop firing when the mouse button is released
            isFiring = false;
            isSwitchingSprites = false;
            StopAllCoroutines(); // Stop switching sprites
            playerSpriteRenderer.sprite = idleSprite; // Reset to idle sprite
        }
    }

    private void Shoot()
    {
        // Instantiate the bullet at the shooting point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Set the bullet's velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;  // Ensure the z-coordinate is zero for 2D

            Vector2 shootDirection = (mousePosition - shootingPoint.position).normalized;
            rb.velocity = shootDirection * bulletSpeed;
        }
    }

    private IEnumerator SwitchSpritesBackAndForth()
    {
        isSwitchingSprites = true;
        isFiring = true;

        while (isFiring)
        {
            // Switch to the shooting sprite
            playerSpriteRenderer.sprite = shootingSprite;
            yield return new WaitForSeconds(spriteSwitchInterval);

            // Switch to the idle sprite
            playerSpriteRenderer.sprite = idleSprite;
            yield return new WaitForSeconds(spriteSwitchInterval);
        }

        // Once the firing stops, reset to idle
        isSwitchingSprites = false;
    }
}