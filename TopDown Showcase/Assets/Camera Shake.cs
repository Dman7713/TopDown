using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Method to shake the camera
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition; // Store the original position

        float elapsed = 0f; // Time elapsed

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // Random x offset
            float y = Random.Range(-1f, 1f) * magnitude; // Random y offset

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z); // Set the new position

            elapsed += Time.deltaTime; // Increase elapsed time
            yield return null; // Wait for the next frame
        }

        transform.localPosition = originalPosition; // Reset to the original position
    }
}