using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hitPoints = 1; // Number of hits required to destroy this object

    // References to item prefabs
    public GameObject itemPrefab1;
    public float chanceToSpawnItem1 = 0.5f; // Chance of spawning item 1

    public GameObject itemPrefab2;
    public float chanceToSpawnItem2 = 0.5f; // Chance of spawning item 2

    public GameObject deathEffectPrefab; // Reference to the particle system prefab
    public float deathEffectDuration = 2f; // Duration for which the death effect plays

    private bool isDead = false; // Track if the enemy is already dead

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isDead)
        {
            hitPoints--;

            if (hitPoints <= 0)
            {
                HandleDeath(); // Handle the enemy death logic
            }
        }
    }

    private void HandleDeath()
    {
        if (isDead) return; // Prevent double processing

        isDead = true; // Mark as dead to prevent further processing

        SpawnLoot();
        PlayDeathEffect();
        Destroy(gameObject); // Destroy the enemy object
    }

    private void SpawnLoot()
    {
        float randomValue = Random.value;

        if (randomValue < chanceToSpawnItem1)
        {
            Instantiate(itemPrefab1, transform.position, Quaternion.identity);
        }
        else if (randomValue < chanceToSpawnItem1 + chanceToSpawnItem2)
        {
            Instantiate(itemPrefab2, transform.position, Quaternion.identity);
        }
    }

    private void PlayDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffectDuration);
        }
    }
}
