using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 10;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 1;
            CheckHealth(); // Check health after taking damage
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 1;
            CheckHealth(); // Check health after taking damage
        }
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            RestartGame(); // Restart the game if health is 0
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle trigger events if needed
    }
}