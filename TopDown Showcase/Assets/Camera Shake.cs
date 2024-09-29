using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Duration of the shake
    public float shakeMagnitude = 0.5f; // Magnitude of the shake

    private Vector3 originalPosition; // Store the original position of the camera

    private void Start()
    {
        originalPosition = transform.position; // Save the original position of the camera
    }

    // Public method to trigger camera shake
    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f; // Track elapsed time

        while (elapsed < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude); // Random X offset
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude); // Random Y offset

            // Update the camera position
            transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);

            elapsed += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        // Reset the camera position to original
        transform.position = originalPosition;
    }
}
