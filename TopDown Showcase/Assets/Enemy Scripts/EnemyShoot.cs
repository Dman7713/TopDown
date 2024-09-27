using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    float shootSpeed = 10f;
    [SerializeField]
    float bulletLifetime = 2.0f;
    float timer = 0;
    [SerializeField]
    float shootDelay = 0.5f;
    [SerializeField]
    Transform firePoint; // Custom fire point

    [SerializeField]
    SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    [SerializeField]
    Sprite shootingSprite; // Sprite to use while shooting
    private Sprite originalSprite; // Store the original sprite

    [SerializeField]
    float shootingSpriteDuration = 1f; // Duration the shooting sprite appears

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (firePoint == null)
        {
            Debug.LogError("Fire point is not assigned in the EnemyShoot script.");
        }

        // Store the original sprite
        originalSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > shootDelay)
        {
            timer = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        if (player != null)
        {
            Vector3 shootDir = player.transform.position - (firePoint != null ? firePoint.position : transform.position);
            shootDir.Normalize();

            // Instantiate the bullet
            GameObject bullet = Instantiate(prefab, firePoint != null ? firePoint.position : transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = shootDir * shootSpeed;
            Destroy(bullet, bulletLifetime);

            // Change the sprite immediately when the bullet is fired
            StartCoroutine(ChangeSpriteTemporarily());
        }
    }

    private IEnumerator ChangeSpriteTemporarily()
    {
        spriteRenderer.sprite = shootingSprite; // Change to shooting sprite
        yield return new WaitForSeconds(shootingSpriteDuration); // Wait for the customizable duration
        spriteRenderer.sprite = originalSprite; // Revert to original sprite
    }
}
