using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 20; // Amount of health this pickup gives
    public AudioClip pickupSound; // Sound to play on pickup
    private AudioSource audioSource;

    private void Start()
    {
        // Add an AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the AudioSource does not play on awake
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healthAmount); // Heal the player
                PlayPickupSoundAndDestroy(); // Play sound and destroy object after sound
            }
        }
    }

    private void PlayPickupSoundAndDestroy()
    {
        // Play the sound if a sound clip is assigned
        if (pickupSound != null && audioSource != null)
        {
            // Play the pickup sound and destroy after it finishes
            audioSource.PlayOneShot(pickupSound);
            // Disable the sprite renderer and collider to "hide" the object but allow sound to play
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            // Destroy the object after the sound has played
            Destroy(gameObject, pickupSound.length);
        }
        else
        {
            // If no sound, destroy immediately
            Destroy(gameObject);
        }
    }
}
