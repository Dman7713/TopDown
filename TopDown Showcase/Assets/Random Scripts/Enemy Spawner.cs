using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Boss prefabs
    public GameObject boss1Prefab;
    public GameObject boss2Prefab;
    public GameObject boss3Prefab;

    // Boss spawn times (in seconds)
    public float boss1SpawnTime = 60f;
    public float boss2SpawnTime = 120f;
    public float boss3SpawnTime = 180f;

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

    public float initialSpawnInterval = 3f;
    public float minimumSpawnInterval = 0.5f;
    public float totalSpawnTime = 3 * 60f;

    public Collider2D spawnArea;

    public float enemyLifetime = 3f * 60f;

    public GameObject deathEffectPrefab;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool canSpawn = true;

    // Flags to track if bosses have been spawned
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;
    private bool boss3Spawned = false;

    // Reference to WinScreenManager
    private WinScreenManager winScreenManager;

    // Track third boss instance
    private GameObject thirdBossInstance;

    private void Start()
    {
        // Find the WinScreenManager in the scene
        winScreenManager = FindObjectOfType<WinScreenManager>();
        if (winScreenManager == null)
        {
            Debug.LogError("WinScreenManager not found in the scene!");
        }

        // Start the enemy spawning coroutine
        StartCoroutine(SpawnEnemiesOverTime());
        StartCoroutine(DestroyEnemiesAfterTime(enemyLifetime));
    }

    private IEnumerator SpawnEnemiesOverTime()
    {
        float elapsedTime = 0f;
        float spawnInterval = initialSpawnInterval;

        while (elapsedTime < totalSpawnTime)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);

            elapsedTime += spawnInterval;
            float progress = elapsedTime / totalSpawnTime;
            spawnInterval = Mathf.Lerp(initialSpawnInterval, minimumSpawnInterval, progress);

            // Check for boss spawns at designated times
            float currentTime = elapsedTime;
            if (!boss1Spawned && currentTime >= boss1SpawnTime)
            {
                SpawnBoss(boss1Prefab);
                boss1Spawned = true;
            }
            else if (!boss2Spawned && currentTime >= boss2SpawnTime)
            {
                SpawnBoss(boss2Prefab);
                boss2Spawned = true;
            }
            else if (!boss3Spawned && currentTime >= boss3SpawnTime)
            {
                thirdBossInstance = SpawnBoss(boss3Prefab); // Track the third boss
                boss3Spawned = true;
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = GetRandomPrefab();
        if (enemyPrefab != null)
        {
            Vector2 randomPosition = GetRandomBorderPositionOfSpawner();
            GameObject enemyInstance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            activeEnemies.Add(enemyInstance);
        }
    }

    private GameObject SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab != null)
        {
            Vector2 randomPosition = GetRandomBorderPositionOfSpawner();
            GameObject bossInstance = Instantiate(bossPrefab, randomPosition, Quaternion.identity);
            activeEnemies.Add(bossInstance);

            // Return the boss instance to track it
            return bossInstance;
        }
        return null;
    }

    private GameObject GetRandomPrefab()
    {
        float totalChance = spawnChance1 + spawnChance2 + spawnChance3 + spawnChance4 + spawnChance5;
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        if (enemyPrefab1 != null)
        {
            cumulativeChance += spawnChance1;
            if (randomValue <= cumulativeChance) return enemyPrefab1;
        }

        if (enemyPrefab2 != null)
        {
            cumulativeChance += spawnChance2;
            if (randomValue <= cumulativeChance) return enemyPrefab2;
        }

        if (enemyPrefab3 != null)
        {
            cumulativeChance += spawnChance3;
            if (randomValue <= cumulativeChance) return enemyPrefab3;
        }

        if (enemyPrefab4 != null)
        {
            cumulativeChance += spawnChance4;
            if (randomValue <= cumulativeChance) return enemyPrefab4;
        }

        if (enemyPrefab5 != null)
        {
            cumulativeChance += spawnChance5;
            if (randomValue <= cumulativeChance) return enemyPrefab5;
        }

        return null; // Fallback, shouldn't hit this if chances are set correctly
    }

    private Vector2 GetRandomBorderPositionOfSpawner()
    {
        Bounds bounds = spawnArea.bounds;
        int edge = Random.Range(0, 4);
        Vector2 randomPosition = Vector2.zero;

        switch (edge)
        {
            case 0: // Top edge
                randomPosition = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.max.y);
                break;
            case 1: // Bottom edge
                randomPosition = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y);
                break;
            case 2: // Left edge
                randomPosition = new Vector2(bounds.min.x, Random.Range(bounds.min.y, bounds.max.y));
                break;
            case 3: // Right edge
                randomPosition = new Vector2(bounds.max.x, Random.Range(bounds.min.y, bounds.max.y));
                break;
        }

        return randomPosition;
    }

    private IEnumerator DestroyEnemiesAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        canSpawn = false;

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                if (deathEffectPrefab != null)
                {
                    Instantiate(deathEffectPrefab, enemy.transform.position, Quaternion.identity);
                }
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();
    }

    private void Update()
    {
        // Check if the third boss is destroyed to trigger the win screen
        if (thirdBossInstance == null && boss3Spawned && winScreenManager != null)
        {
            winScreenManager.ActivateWinScreen(); // Trigger the win UI when third boss is destroyed
        }
    }
}
