using System.Collections; // Add this line
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Time in seconds before the object is destroyed
    public float lifetime = 5f;

    private void Start()
    {
        // Start the coroutine to destroy the object after the specified lifetime
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(time);

        // Destroy the object
        Destroy(gameObject);
    }
}
