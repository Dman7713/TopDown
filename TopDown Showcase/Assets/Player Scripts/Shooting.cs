using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float fireRate = 0.2f;
    public float bulletSpeed = 20f;

    [Header("Bullet Spread Settings")]
    public float bulletSpread = 5f;

    [Header("Shooting Settings")]
    public int bulletsPerClick = 1;

    public SpriteRenderer playerSpriteRenderer;
    public Sprite idleSprite;
    public Sprite shootingSprite;
    public float spriteSwitchInterval = 0.1f;
    public int currentAmmo = 50; // Set initial ammo
    public int maxAmmoSize = 100;

    public TMP_Text ammoText;
    public AudioSource shootSound;
    public AudioSource outOfAmmoSound;

    [Header("Ammo Replenishment Settings")]
    public int ammoReplenishAmount = 5;
    public float ammoReplenishInterval = 5f; // Ensure this is set to your desired interval

    private float nextFireTime = 0f;
    private bool isFiring = false;
    private bool isSwitchingSprites = false;

    private void Start()
    {
        UpdateAmmoUI();
        StartCoroutine(AutoReplenishAmmo());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                for (int i = 0; i < bulletsPerClick; i++)
                {
                    Shoot();
                    nextFireTime = Time.time + (fireRate / bulletsPerClick);
                }

                if (!isSwitchingSprites)
                {
                    StartCoroutine(SwitchSpritesBackAndForth());
                }
            }
            else if (outOfAmmoSound != null)
            {
                outOfAmmoSound.Play();
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            isFiring = false;
            isSwitchingSprites = false;
            StopAllCoroutines();
            playerSpriteRenderer.sprite = idleSprite;
        }
    }

    private void Shoot()
    {
        currentAmmo--;

        if (shootSound != null)
        {
            shootSound.Play();
        }

        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float spread = Random.Range(-bulletSpread, bulletSpread);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
            Vector2 shootDirection = spreadRotation * shootingPoint.right;
            rb.velocity = shootDirection * bulletSpeed;
        }

        UpdateAmmoUI();
    }

    private IEnumerator SwitchSpritesBackAndForth()
    {
        isSwitchingSprites = true;
        isFiring = true;

        while (isFiring)
        {
            playerSpriteRenderer.sprite = shootingSprite;
            yield return new WaitForSeconds(spriteSwitchInterval);
            playerSpriteRenderer.sprite = idleSprite;
            yield return new WaitForSeconds(spriteSwitchInterval);
        }

        isSwitchingSprites = false;
    }

    private IEnumerator AutoReplenishAmmo()
    {
        while (true)
        {
            yield return new WaitForSeconds(ammoReplenishInterval);

            if (currentAmmo < maxAmmoSize)
            {
                RefillAmmo(ammoReplenishAmount);
            }
        }
    }

    public void RefillAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmoSize)
        {
            currentAmmo = maxAmmoSize;
        }
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo;
        }
    }
}
