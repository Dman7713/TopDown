using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10; // Amount of ammo this pickup gives

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
                // Destroy the ammo pickup after collection
                Destroy(gameObject);
            }
        }
    }
}