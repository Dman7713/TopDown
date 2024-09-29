using System.Collections;
using UnityEngine;
using TMPro; // Import TextMesh Pro namespace

public class ButtonFade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToFade; // Reference to the TextMesh Pro component to fade
    private CanvasGroup canvasGroup; // Reference to the CanvasGroup component

    private void Start()
    {
        // Get the CanvasGroup component
        canvasGroup = GetComponent<CanvasGroup>();

        // Ensure the text is fully visible at the start
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; // Set initial alpha to 1 (fully visible)
        }
    }

    // Method to fade in the text
    public void FadeIn(float duration)
    {
        StartCoroutine(FadeCoroutine(0f, 1f, duration)); // Fade from 0 to 1
    }

    // Method to fade out the text
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeCoroutine(1f, 0f, duration)); // Fade from 1 to 0
    }

    // Coroutine to handle the fading process
    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        if (canvasGroup == null) yield break;

        float elapsedTime = 0f;

        // Gradually change the alpha value over time
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = newAlpha;

            // Optionally, if you're using TextMesh Pro and want to change its visibility as well:
            if (textToFade != null)
            {
                Color textColor = textToFade.color;
                textColor.a = newAlpha; // Update the alpha value
                textToFade.color = textColor; // Set the updated color
            }

            yield return null; // Wait until the next frame
        }

        // Ensure the final alpha is set
        canvasGroup.alpha = endAlpha;
        if (textToFade != null)
        {
            Color finalTextColor = textToFade.color;
            finalTextColor.a = endAlpha; // Set the final alpha value
            textToFade.color = finalTextColor; // Set the updated color
        }
    }
}
