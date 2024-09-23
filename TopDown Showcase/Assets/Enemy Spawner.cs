using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PrefabSlot
    {
        public GameObject prefab;   // The enemy prefab to spawn
        [Range(0f, 1f)]
        public float spawnChance;   // The chance for this prefab to spawn (value between 0 and 1)
    }

    // Individual slots for up to 5 different prefabs
    [SerializeField] private PrefabSlot prefab1;
    [SerializeField] private PrefabSlot prefab2;
    [SerializeField] private PrefabSlot prefab3;
    [SerializeField] private PrefabSlot prefab4;
    [SerializeField] private PrefabSlot prefab5;

    [SerializeField]
    private int baseNumberOfPrefabs = 5; // Base number of prefabs to spawn per batch
    [SerializeField]
    private int maxRounds = 5; // Maximum number of rounds
    [SerializeField]
    private Vector2 spawnAreaSize = new Vector2(5f, 5f); // Size of the spawn area (width, height)
    [SerializeField]
    private float playerSafeRadius = 2f; // Radius around the player where enemies cannot spawn
    [SerializeField]
    private float initialDelay = 3f; // Delay before the first batch of enemies spawns

    private int currentRound = 1; // Track the current round
    private Transform player; // Reference to the player transform

    private void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }

        // Start the coroutine after the initial delay
        StartCoroutine(SpawnEnemiesForRound());
    }

    private IEnumerator SpawnEnemiesForRound()
    {
        // Wait for the initial delay before spawning enemies
        yield return new WaitForSeconds(initialDelay);

        while (currentRound <= maxRounds)
        {
            // Calculate the number of enemies to spawn based on the round
            int numberOfPrefabs = baseNumberOfPrefabs * currentRound;

            // Spawn the specified number of prefabs
            Debug.Log($"Spawning {numberOfPrefabs} enemies for round {currentRound}.");
            for (int i = 0; i < numberOfPrefabs; i++)
            {
                Vector2 randomPosition = GetRandomPositionWithinArea();
                GameObject enemyPrefab = GetRandomPrefab();
                Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            }

            // Wait until all enemies are defeated before starting the next round
            yield return new WaitUntil(AllEnemiesDefeated);

            // Move to the next round
            currentRound++;

            // Optional: Delay before the next round starts
            yield return new WaitForSeconds(2f);
        }

        Debug.Log("All rounds completed!");
        // Optional: Add logic here for what happens after all rounds are finished
    }

    private bool AllEnemiesDefeated()
    {
        // Check if there are any active enemies in the scene
        bool allDefeated = GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
        if (allDefeated)
        {
            Debug.Log("All enemies defeated.");
        }
        return allDefeated;
    }

    private Vector2 GetRandomPositionWithinArea()
    {
        Vector2 spawnPosition;
        int attempts = 0;

        // Try to find a valid spawn position
        do
        {
            // Get the center of the spawn area (the GameObject's position)
            Vector2 center = transform.position;

            // Calculate a random position within the defined area
            float randomX = Random.Range(center.x - spawnAreaSize.x / 2, center.x + spawnAreaSize.x / 2);
            float randomY = Random.Range(center.y - spawnAreaSize.y / 2, center.y + spawnAreaSize.y / 2);

            spawnPosition = new Vector2(randomX, randomY);
            attempts++;

            // Break if we've tried a number of times to prevent an infinite loop
            if (attempts > 10)
            {
                Debug.LogWarning("Could not find a valid spawn position.");
                return spawnPosition; // Return the last attempted position, even if it's invalid
            }

        } while (Vector2.Distance(spawnPosition, player.position) < playerSafeRadius);

        return spawnPosition;
    }

    private GameObject GetRandomPrefab()
    {
        // Generate a random number between 0 and 1
        float randomValue = Random.value;
        float cumulativeChance = 0f;

        // Create an array of prefabs and their spawn chances
        PrefabSlot[] prefabSlots = { prefab1, prefab2, prefab3, prefab4, prefab5 };

        // Iterate through all prefabs and their spawn chances
        foreach (PrefabSlot prefabSlot in prefabSlots)
        {
            if (prefabSlot.prefab != null) // Only consider prefabs that are not null
            {
                cumulativeChance += prefabSlot.spawnChance;

                // If the random value is less than the cumulative chance, select this prefab
                if (randomValue <= cumulativeChance)
                {
                    return prefabSlot.prefab;
                }
            }
        }

        // In case no prefab was selected (due to floating-point precision), return the last valid one
        foreach (PrefabSlot prefabSlot in prefabSlots)
        {
            if (prefabSlot.prefab != null)
            {
                return prefabSlot.prefab;
            }
        }

        // Default return to avoid null errors (in case no prefab is found)
        return null;
    }
}