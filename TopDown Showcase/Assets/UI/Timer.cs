using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMesh Pro components

public class Timer : MonoBehaviour
{
    public float startTime = 60f; // Time the timer starts at (in seconds)
    public float currentTime; // Change this to public so it can be accessed
    public TextMeshProUGUI timerText; // Reference to the TextMesh Pro component

    private void Start()
    {
        // Set the timer to start at the desired time
        currentTime = startTime;
        UpdateTimerUI();
    }

    private void Update()
    {
        // Decrease the current time
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Decrease time by the time that passed since the last frame
            if (currentTime < 0)
            {
                currentTime = 0; // Clamp to 0 so it doesn't go negative
            }
            UpdateTimerUI(); // Update the UI each frame
        }
    }

    // Update the UI TextMesh Pro component with the current time in minutes and seconds
    private void UpdateTimerUI()
    {
        // Format the time as minutes and seconds (mm:ss)
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Update the text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
