using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Boss prefabs
    public GameObject boss1Prefab; // Boss to spawn at specified time
    public GameObject boss2Prefab; // Boss to spawn at specified time
    public GameObject boss3Prefab; // Boss to spawn at specified time

    // Boss spawn times (in seconds)
    public float boss1SpawnTime = 60f; // Spawn time for boss 1
    public float boss2SpawnTime = 120f; // Spawn time for boss 2
    public float boss3SpawnTime = 180f; // Spawn time for boss 3

    // Random enemy prefabs and their spawn chances
    public GameObject enemyPrefab1;
    [Range(0f, 100f)]
    public float spawnChance1;

    public GameObject enemyPrefab2;
    [Range(0f, 100f)]
    public float spawnChance2;

    public GameObject enemyPrefab3;
    [Range(0f, 100f)]
    public float spawnChance3;

    public GameObject enemyPrefab4;
    [Range(0f, 100f)]
    public float spawnChance4;

    public GameObject enemyPrefab5;
    [Range(0f, 100f)]
    public float spawnChance5;

    public float initialSpawnInterval = 3f; // Initial time between spawns
    public float minimumSpawnInterval = 0.5f; // Minimum time between spawns
    public float totalSpawnTime = 3 * 60f; // Total time to spawn enemies (3 minutes)

    public Collider2D spawnArea; // The collider that defines the spawn area

    public float enemyLifetime = 3f * 60f; // Time before enemies are destroyed (in seconds)

    public GameObject deathEffectPrefab; // Prefab to instantiate when an enemy dies

    private List<GameObject> activeEnemies = new List<GameObject>(); // Track active enemies
    private bool canSpawn = true; // Flag to control spawning

    // Flags to track if bosses have been spawned
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;
    private bool boss3Spawned = false;

    private void Start()
    {
        // Start the enemy spawning coroutine
        StartCoroutine(SpawnEnemiesOverTime());
        StartCoroutine(DestroyEnemiesAfterTime(enemyLifetime)); // Use configurable enemy lifetime
    }

    private IEnumerator SpawnEnemiesOverTime()
    {
        float elapsedTime = 0f; // Timer for elapsed time
        float spawnInterval = initialSpawnInterval; // Start with the initial spawn interval

        while (elapsedTime < totalSpawnTime)
        {
            // Spawn a random enemy prefab
            SpawnEnemy();

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);

            // Update elapsed time
            elapsedTime += spawnInterval;

            // Gradually decrease the spawn interval
            float progress = elapsedTime / totalSpawnTime; // Normalized progress from 0 to 1
            spawnInterval = Mathf.Lerp(initialSpawnInterval, minimumSpawnInterval, progress); // Smoothly transition to the minimum spawn interval

            // Check for boss spawns at designated times
            float currentTime = elapsedTime;
            if (!boss1Spawned && currentTime >= boss1SpawnTime)
            {
                SpawnBoss(boss1Prefab);
                boss1Spawned = true; // Mark as spawned
            }
            else if (!boss2Spawned && currentTime >= boss2SpawnTime)
            {
                SpawnBoss(boss2Prefab);
                boss2Spawned = true; // Mark as spawned
            }
            else if (!boss3Spawned && currentTime >= boss3SpawnTime)
            {
                SpawnBoss(boss3Prefab);
                boss3Spawned = true; // Mark as spawned
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = GetRandomPrefab();
        if (enemyPrefab != null)
        {
            // Get the bounds of the spawn area
            Bounds bounds = spawnArea.bounds;

            // Generate a random position within the spawn area
            Vector2 randomPosition = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            // Instantiate the enemy prefab at the random position
            GameObject enemyInstance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            activeEnemies.Add(enemyInstance); // Track the spawned enemy
        }
    }

    private GameObject GetRandomPrefab()
    {
        // Calculate total spawn chance
        float totalChance = spawnChance1 + spawnChance2 + spawnChance3 + spawnChance4 + spawnChance5;

        // Generate a random value between 0 and the total chance
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        // Check which prefab to spawn based on the random value
        if (enemyPrefab1 != null)
        {
            cumulativeChance += spawnChance1;
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefab1;
            }
        }

        if (enemyPrefab2 != null)
        {
            cumulativeChance += spawnChance2;
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefab2;
            }
        }

        if (enemyPrefab3 != null)
        {
            cumulativeChance += spawnChance3;
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefab3;
            }
        }

        if (enemyPrefab4 != null)
        {
            cumulativeChance += spawnChance4;
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefab4;
            }
        }

        if (enemyPrefab5 != null)
        {
            cumulativeChance += spawnChance5;
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefab5;
            }
        }

        return null; // Fallback, shouldn't hit this if chances are set correctly
    }

    private void SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab != null)
        {
            // Get the bounds of the spawn area
            Bounds bounds = spawnArea.bounds;

            // Generate a random position within the spawn area
            Vector2 randomPosition = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            // Instantiate the boss prefab at the random position
            GameObject bossInstance = Instantiate(bossPrefab, randomPosition, Quaternion.identity);
            activeEnemies.Add(bossInstance); // Track the spawned boss
        }
    }

    private IEnumerator DestroyEnemiesAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified time

        canSpawn = false; // Stop spawning new enemies

        // Destroy all active enemies in the spawn area
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                // Instantiate the death effect prefab at the enemy's position
                if (deathEffectPrefab != null)
                {
                    Instantiate(deathEffectPrefab, enemy.transform.position, Quaternion.identity);
                }

                Destroy(enemy); // Destroy the enemy
            }
        }

        activeEnemies.Clear(); // Clear the list of active enemies
    }
}
