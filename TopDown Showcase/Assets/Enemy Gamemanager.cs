using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject healthBarPrefab; // Prefab for the health bar
    private GameObject currentHealthBarUI; // Current health bar UI instance
    private Image healthBarFill; // Reference to the fill image of the health bar

    private void Start()
    {
        // Ensure the health bar is hidden initially
        if (currentHealthBarUI != null)
        {
            currentHealthBarUI.SetActive(false);
        }
    }

    public void ShowHealthBar(EnemyController enemy)
    {
        // Create a new health bar UI for the enemy
        currentHealthBarUI = Instantiate(healthBarPrefab);
        currentHealthBarUI.transform.SetParent(GameObject.Find("Canvas").transform, false);

        // Position the health bar above the enemy
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
        currentHealthBarUI.transform.position = new Vector3(screenPosition.x, screenPosition.y + 50, screenPosition.z);

        // Get the fill image component
        healthBarFill = currentHealthBarUI.GetComponentInChildren<Image>();

        // Update the health bar according to the enemy's hit points
        UpdateHealthBar(enemy.hitPoints);
    }

    public void UpdateHealthBar(int hitPoints)
    {
        if (healthBarFill != null)
        {
            // Assuming hitPoints is between 0 and max hit points (1 for this example)
            float healthPercentage = (float)hitPoints / 1;
            healthBarFill.fillAmount = healthPercentage;
        }
    }

    public void HideHealthBar()
    {
        if (currentHealthBarUI != null)
        {
            Destroy(currentHealthBarUI);
        }
    }
}
