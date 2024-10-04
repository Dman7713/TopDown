using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHitPoints = 3; // Max hit points for the enemy
    public int currentHitPoints; // Current hit points
    public int damageAmount = 1; // Amount of damage the enemy deals

    // References to item prefabs
    public GameObject itemPrefab1;
    public float chanceToSpawnItem1 = 0.5f; // Chance of spawning item 1

    public GameObject itemPrefab2;
    public float chanceToSpawnItem2 = 0.5f; // Chance of spawning item 2

    public GameObject deathEffectPrefab; // Reference to the particle system prefab
    public float deathEffectDuration = 2f; // Duration for which the death effect plays

    public Sprite damagedSprite; // The sprite to change to when damaged
    public float damageSpriteDuration = 0.5f; // How long to display the damaged sprite

    public GameObject bloodPrefab; // Blood sprite prefab to instantiate when the enemy dies

    public AudioClip deathSound; // Sound to play on death
    private AudioSource audioSource; // Reference to the AudioSource component

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite; // Store the original sprite
    private bool isDead = false; // Track if the enemy is already dead

    private EnemyHealthBar healthBar; // Reference to the EnemyHealthBar script

    private void Start()
    {
        // Initialize current hit points
        currentHitPoints = maxHitPoints;

        // Cache the SpriteRenderer and original sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource if missing

        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the enemy object!");
        }

        // Initialize health bar
        healthBar = GetComponentInChildren<EnemyHealthBar>(); // Get the EnemyHealthBar component
        if (healthBar != null)
        {
            healthBar.Initialize(maxHitPoints); // Initialize the health bar with max health
        }
        else
        {
            Debug.LogError("EnemyHealthBar not found!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isDead)
        {
            currentHitPoints--; // Decrease current hit points

            // Update the health bar
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(currentHitPoints); // Update health bar
            }

            if (currentHitPoints > 0)
            {
                StartCoroutine(HandleDamageSprite());
            }

            if (currentHitPoints <= 0)
            {
                HandleDeath(); // Handle the enemy death logic
            }
        }
        else if (collision.gameObject.CompareTag("Player") && !isDead) // Assuming you have a "Player" tag
        {
            // Deal damage to the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount); // Apply damage to the player
            }
        }
    }

    private void HandleDeath()
    {
        if (isDead) return; // Prevent double processing

        isDead = true; // Mark as dead to prevent further processing

        // Play death sound and destroy the AudioSource after it's done
        PlayDeathSound();

        // Add 100 points to the score
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(100); // Add 100 points to the score
        }

        SpawnLoot();
        PlayDeathEffect();
        SpawnBlood(); // Spawn blood sprite on the ground

        Destroy(gameObject); // Immediately destroy the enemy
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;

            // Add an AudioSource component and set its clip and volume
            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = deathSound;
            audioSource.volume = 1.0f; // Set the volume here (1.0 for full volume)
            audioSource.spatialBlend = 0.0f; // Play in 2D (set to 1.0 for 3D sound)

            // Play the sound and destroy the temporary GameObject after the sound finishes
            audioSource.Play();
            Destroy(tempAudio, deathSound.length);
        }
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

    private void SpawnBlood()
    {
        if (bloodPrefab != null)
        {
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        }
    }

    private System.Collections.IEnumerator HandleDamageSprite()
    {
        if (spriteRenderer != null && damagedSprite != null)
        {
            // Change to the damaged sprite
            spriteRenderer.sprite = damagedSprite;

            // Wait for the specified duration
            yield return new WaitForSeconds(damageSpriteDuration);

            // Revert back to the original sprite
            spriteRenderer.sprite = originalSprite;
        }
    }
}
