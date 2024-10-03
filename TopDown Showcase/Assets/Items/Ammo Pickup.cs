using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10; // Amount of ammo this pickup gives
    public AudioClip pickupSound; // Sound to play on pickup
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private Collider2D pickupCollider;

    private void Start()
    {
        // Add an AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the AudioSource does not play on awake and set some default properties
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Cache the SpriteRenderer and Collider2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickupCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null || pickupCollider == null)
        {
            Debug.LogError("Missing SpriteRenderer or Collider2D on the AmmoPickup object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Get the Gun component from the player
            Gun gun = other.GetComponent<Gun>();
            if (gun != null)
            {
                // Refill ammo in the player's gun
                gun.RefillAmmo(ammoAmount);

                // Play the pickup sound and destroy the object after the sound plays
                PlayPickupSoundAndDestroy();
            }
        }
    }

    private void PlayPickupSoundAndDestroy()
    {
        // Hide the pickup visually and disable its collider
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Hide the sprite
        }
        if (pickupCollider != null)
        {
            pickupCollider.enabled = false; // Disable the collider
        }

        // Play the sound if a sound clip is assigned, otherwise destroy the object immediately
        if (pickupSound != null && audioSource != null)
        {
            // Play the pickup sound
            audioSource.PlayOneShot(pickupSound);

            // Destroy the object after the sound has finished playing
            Destroy(gameObject, pickupSound.length);
        }
        else
        {
            // If no sound is assigned, destroy the object immediately
            Destroy(gameObject);
        }
    }
}
