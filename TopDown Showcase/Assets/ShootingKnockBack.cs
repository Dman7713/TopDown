using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 5f; // Base force applied to knock the player back
    public float knockbackSpeedMultiplier = 1.0f; // Multiplier for knockback speed
    public float knockbackDuration = 0.1f; // Duration of each knockback effect
    public float shakeDuration = 0.2f; // Duration of the camera shake
    public float shakeMagnitude = 0.1f; // Magnitude of the camera shake

    public Camera camera; // Reference to the camera (assignable in Inspector)

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private CameraShake cameraShake; // Reference to the CameraShake script

    private void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Get the CameraShake component from the assigned camera
        cameraShake = camera.GetComponent<CameraShake>();
    }

    private void Update()
    {
        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            // Calculate the direction to knock back
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Set the z-coordinate to 0 for 2D

            // Determine the knockback direction (opposite of mouse position)
            Vector2 knockbackDirection = (Vector2)(transform.position - mousePosition).normalized;

            // Apply knockback force
            ApplyKnockback(knockbackDirection);
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        // Calculate final knockback force based on multiplier
        float finalKnockbackForce = knockbackForce * knockbackSpeedMultiplier;

        // Apply the knockback force to the player
        rb.AddForce(direction * finalKnockbackForce, ForceMode2D.Impulse);

        // Start camera shake
        StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

        // Start a coroutine to handle repeated knockbacks
        StartCoroutine(HandleKnockback());
    }

    private IEnumerator HandleKnockback()
    {
        // Wait for the duration of the knockback
        yield return new WaitForSeconds(knockbackDuration);

        // Prevent further knockbacks until the mouse is released
        while (Input.GetMouseButton(0))
        {
            yield return null; // Wait for the next frame
        }
    }
}