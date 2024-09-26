using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;        // The enemy prefab to spawn
    public float spawnInterval = 2f;      // Time between spawns
    public Collider2D spawnArea;          // The collider that defines the spawn area

    private void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    private void SpawnEnemy()
    {
        // Get the bounds of the spawn area
        Bounds bounds = spawnArea.bounds;

        // Generate a random position within the bounds
        Vector2 randomPosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        // Instantiate the enemy prefab at the random position
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}