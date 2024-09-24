using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public Vector3 offset;     // Offset of the camera from the player

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

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}