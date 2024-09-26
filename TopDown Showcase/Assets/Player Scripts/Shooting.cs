using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import this for TextMeshPro

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
    public int currentAmmo = 0; // Current ammo count
    public int maxAmmoSize = 100; // Maximum ammo capacity

    public TMP_Text ammoText; // Use TMP_Text for TextMeshPro

    private float nextFireTime = 0f; // Time at which the next shot can be fired
    private bool isFiring = false; // Is the player holding down the fire button?
    private bool isSwitchingSprites = false; // Is the sprite switching coroutine running?

    private void Start()
    {
        UpdateAmmoUI(); // Initialize the ammo UI at start
    }

    private void Update()
    {
        // Check if the left mouse button is held down and the gun can fire
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
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
        // Decrease ammo count
        currentAmmo--;

        // Instantiate the bullet at the shooting point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Set the bullet's velocity to be straight in the direction the shooting point is facing
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Use the shooting point's right direction for 2D
            Vector2 shootDirection = shootingPoint.right;
            rb.velocity = shootDirection * bulletSpeed;
        }

        UpdateAmmoUI(); // Update the UI after shooting
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

    // Method to refill ammo
    public void RefillAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmoSize)
        {
            currentAmmo = maxAmmoSize; // Cap ammo to max size
        }
        UpdateAmmoUI(); // Update the UI when ammo is refilled
    }

    // Method to update the ammo UI text
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo; // Update the displayed ammo count
        }
    }
}