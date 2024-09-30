using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefabs and their respective spawn chances
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

    public float spawnInterval = 2f;      // Time between spawns
    public Collider2D spawnArea;          // The collider that defines the spawn area

    public int enemiesPerWave = 5;        // Number of enemies in the first wave
    public float waveInterval = 5f;       // Time between waves
    public float enemyMultiplier = 1.5f;  // Multiplier for enemies each wave

    public float enemyLifetime = 3f * 60f; // Time before enemies are destroyed (in seconds)

    public GameObject deathEffectPrefab;   // Prefab to instantiate when an enemy dies

    private int currentWave = 1;          // The current wave number
    private List<GameObject> activeEnemies = new List<GameObject>(); // Track active enemies

    private void Start()
    {
        // Start the wave spawning coroutine
        StartCoroutine(SpawnWaves());
        StartCoroutine(DestroyEnemiesAfterTime(enemyLifetime)); // Use configurable enemy lifetime
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            // Spawn the enemies for the current wave
            yield return StartCoroutine(SpawnEnemiesForWave(currentWave));

            // Increase the number of enemies for the next wave
            currentWave++;
            enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * enemyMultiplier);

            // Wait for the wave interval before starting the next wave
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private IEnumerator SpawnEnemiesForWave(int wave)
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval between spawns
        }
    }

    private void SpawnEnemy()
    {
        // Get the bounds of the spawn area
        Bounds bounds = spawnArea.bounds;

        // Generate a random position on the border of the spawn area
        Vector2 randomPosition = GetRandomBorderPosition(bounds);

        // Randomly select an enemy prefab based on their spawn chances
        GameObject enemyPrefab = GetRandomPrefab();
        if (enemyPrefab != null)
        {
            // Instantiate the enemy prefab at the random position
            GameObject enemyInstance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            activeEnemies.Add(enemyInstance); // Track the spawned enemy
        }
    }

    private Vector2 GetRandomBorderPosition(Bounds bounds)
    {
        // Randomly decide whether to spawn on the horizontal or vertical border
        bool spawnOnHorizontalEdge = Random.Range(0f, 1f) < 0.5f;

        if (spawnOnHorizontalEdge)
        {
            // Spawn on the top or bottom border
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(0f, 1f) < 0.5f ? bounds.min.y : bounds.max.y; // Choose between bottom or top
            return new Vector2(x, y);
        }
        else
        {
            // Spawn on the left or right border
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float x = Random.Range(0f, 1f) < 0.5f ? bounds.min.x : bounds.max.x; // Choose between left or right
            return new Vector2(x, y);
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

    private IEnumerator DestroyEnemiesAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified time

        // Destroy all active enemies in the spawn area
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null && spawnArea.OverlapPoint(enemy.transform.position))
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
