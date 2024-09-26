using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10f;
    [SerializeField]
    float dashSpeed = 20f;      // Speed during the dash
    [SerializeField]
    float dashDuration = 0.2f;   // Duration of the dash
    [SerializeField]
    float dashCooldown = 1f;     // Cooldown time between dashes

    private bool isDashing = false; // Flag to check if currently dashing
    private bool canDash = true;     // Flag to check if player can dash

    private Rigidbody2D rb;
    private Vector2 moveDirection; // Store the current movement direction

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for dash input
        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
        {
            StartDash();
        }

        // If not dashing, handle movement
        if (!isDashing)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        // Check for horizontal and vertical input
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        // Store the movement direction
        moveDirection = new Vector2(xInput, yInput).normalized;

        // Set velocity based on input
        rb.velocity = moveDirection * moveSpeed;
    }

    void StartDash()
    {
        if (moveDirection != Vector2.zero) // Only dash if there is a movement direction
        {
            isDashing = true; // Set dashing flag
            canDash = false;  // Prevent further dashes
            StartCoroutine(Dash(moveDirection));
        }
    }

    IEnumerator Dash(Vector2 direction)
    {
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            rb.velocity = direction * dashSpeed; // Set dash velocity
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        rb.velocity = Vector2.zero; // Reset velocity after dash
        isDashing = false; // Reset dashing state

        // Start cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true; // Allow dashing again
    }
}
