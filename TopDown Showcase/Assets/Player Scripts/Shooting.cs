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

    [Header("Bullet Spread Settings")]
    [Tooltip("Maximum angle for bullet spread in degrees.")]
    public float bulletSpread = 5f; // Maximum angle for bullet spread

    [Header("Shooting Settings")]
    [Tooltip("Number of bullets to shoot per click.")]
    public int bulletsPerClick = 1; // Number of bullets shot per click

    public SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer
    public Sprite idleSprite; // The player's idle or walking sprite
    public Sprite shootingSprite; // The sprite to show when the player is shooting
    public float spriteSwitchInterval = 0.1f; // Time to switch between sprites
    public int currentAmmo = 0; // Current ammo count
    public int maxAmmoSize = 100; // Maximum ammo capacity

    public TMP_Text ammoText; // Use TMP_Text for TextMeshPro
    public AudioSource shootSound; // Audio source for gunshot sound
    public AudioSource outOfAmmoSound; // Audio source for out of ammo sound

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
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                // Rapid fire when the mouse button is held down
                for (int i = 0; i < bulletsPerClick; i++)
                {
                    Shoot();
                    nextFireTime = Time.time + (fireRate / bulletsPerClick); // Adjust fire rate based on bullets per click
                }

                // Start switching between the sprites if not already doing so
                if (!isSwitchingSprites)
                {
                    StartCoroutine(SwitchSpritesBackAndForth());
                }
            }
            else
            {
                // Play out-of-ammo sound if ammo is zero and sound is assigned
                if (outOfAmmoSound != null)
                {
                    Debug.Log("Out of ammo sound should play"); // Debug log
                    outOfAmmoSound.Play();
                }
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

        // Play the shooting sound if it's assigned
        if (shootSound != null)
        {
            shootSound.Play();
        }

        // Instantiate the bullet at the shooting point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Set the bullet's velocity with spread
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculate a random angle for the bullet spread
            float spread = Random.Range(-bulletSpread, bulletSpread);
            // Create a rotation based on the spread
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
            // Apply the spread rotation to the shooting direction
            Vector2 shootDirection = spreadRotation * shootingPoint.right;
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
