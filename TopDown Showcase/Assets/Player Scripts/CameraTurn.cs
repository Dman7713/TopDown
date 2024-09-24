using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurn : MonoBehaviour
{
    public float speed = 5f;  // Speed at which the player moves
    public Transform front;    // Assign the front object in the Inspector

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Ensure the z-coordinate is zero for 2D

        // Calculate the direction to the mouse
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Move the player towards the mouse (optional)
        transform.position += direction * speed * Time.deltaTime;

        // Calculate the angle to rotate towards the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the front of the object to face the mouse
        if (front != null)
        {
            front.rotation = Quaternion.Euler(new Vector3(0, 0, angle));  // Align front to the mouse direction
        }

        // Optionally, you can rotate the object itself to face the mouse as well
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}