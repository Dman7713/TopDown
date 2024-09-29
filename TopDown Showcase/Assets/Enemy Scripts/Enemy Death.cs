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

    public Sprite damagedSprite; // The sprite to change to when damaged
    public float damageSpriteDuration = 0.5f; // How long to display the damaged sprite

    public GameObject bloodPrefab; // Blood sprite prefab to instantiate when the enemy dies

    public AudioClip deathSound; // Sound to play on death
    private AudioSource audioSource; // Reference to the AudioSource component

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite; // Store the original sprite
    private bool isDead = false; // Track if the enemy is already dead

    private void Start()
    {
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isDead)
        {
            hitPoints--;

            if (hitPoints > 0)
            {
                StartCoroutine(HandleDamageSprite());
            }

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

        // Play death sound
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
        Destroy(gameObject); // Destroy the enemy object
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound); // Play the death sound
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
