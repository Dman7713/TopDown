using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurn : MonoBehaviour
{
    public float speed = 5f;  // Speed at which the player moves

    void Update()
    {
        // Get the mouse position in the world
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Ensure the z-coordinate is zero for 2D

        // Calculate the direction to the mouse
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Move the player towards the mouse (optional, if you want movement)
        transform.position += direction * speed * Time.deltaTime;

        // Calculate the angle to rotate towards the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the player to face the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));  // Adjust rotation by -90 degrees if necessary
    }
}