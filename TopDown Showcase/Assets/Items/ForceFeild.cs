using System.Collections;
using UnityEngine;

public class ForcefieldItem : MonoBehaviour
{
    public GameObject forcefieldPrefab; // The visual effect to instantiate
    public float duration = 5f; // Duration of the forcefield effect
    public Vector3 offset; // Optional offset from the player's position
    public float forcefieldDestroyDelay = 2f; // Delay before destroying the forcefield prefab

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched the item is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected the forcefield item.");
            // Start coroutine to activate the forcefield
            StartCoroutine(ActivateForcefield(other.transform));
            Destroy(gameObject); // Destroy the forcefield item after use
        }
    }

    private IEnumerator ActivateForcefield(Transform player)
    {
        // Create the forcefield effect at the player's position
        GameObject forcefield = Instantiate(forcefieldPrefab, player.position + offset, Quaternion.identity);
        Debug.Log("Forcefield activated.");

        // Parent the forcefield to the player
        forcefield.transform.SetParent(player);

        // Set the initial position with offset
        forcefield.transform.localPosition = offset;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Destroy the forcefield effect after the duration
        Debug.Log("Destroying forcefield.");
        Destroy(forcefield);

        // Destroy the forcefield prefab after an additional 2 seconds
        yield return new WaitForSeconds(forcefieldDestroyDelay);
        Destroy(forcefield);
    }
}
