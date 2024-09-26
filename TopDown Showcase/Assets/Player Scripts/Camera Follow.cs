using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;            // Reference to the player's transform
    public float smoothSpeed = 0.125f;  // Smoothness of the camera movement
    public Vector3 offset;               // Offset of the camera from the player

    // Camera shake parameters
    public float shakeDuration = 0.5f;   // Duration of the shake (initial click)
    public float shakeMagnitude = 0.1f;   // Magnitude of the shake

    private Vector3 originalPosition;     // Original position of the camera
    private bool isShaking = false;       // Track if shaking is active
    private float shakeTime = 0f;         // Time for shake

    private void Start()
    {
        originalPosition = transform.localPosition; // Store the original position
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned in the CameraFollow script.");
            return;
        }

        // Ensure the offset's Z value is zero for a 2D game
        offset.z = 0;

        // Calculate the desired position with respect to the player's position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Ensure the camera stays on the same Z-axis position (for 2D view)
        desiredPosition.z = transform.position.z;

        // Smoothly interpolate between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply shake effect if active
        if (isShaking)
        {
            smoothedPosition += GetShakeOffset(); // Add shake offset to the smoothed position
        }

        // Update the camera's position
        transform.position = smoothedPosition;

        // Handle mouse input for shaking
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            TriggerShake();
        }

        if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            shakeTime = shakeDuration; // Reset shake time while holding
        }
    }

    // Call this method to trigger the camera shake
    private void TriggerShake()
    {
        isShaking = true;
        shakeTime = shakeDuration; // Set shake time
    }

    private Vector3 GetShakeOffset()
    {
        if (shakeTime > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeTime -= Time.deltaTime;
            return shakeOffset;
        }
        else
        {
            isShaking = false; // Stop shaking
            return Vector3.zero; // Reset offset
        }
    }
}