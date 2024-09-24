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

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (firePoint == null)
        {
            Debug.LogError("Fire point is not assigned in the EnemyShoot script.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > shootDelay)
        {
            timer = 0;
            Vector3 shootDir = player.transform.position - (firePoint != null ? firePoint.position : transform.position);
            shootDir.Normalize();
            GameObject bullet = Instantiate(prefab, firePoint != null ? firePoint.position : transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = shootDir * shootSpeed;
            Destroy(bullet, bulletLifetime);
        }
    }
}