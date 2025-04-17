using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlinkEffect : MonoBehaviour
{
    public static ScreenBlinkEffect Instance { get; private set; }

    [SerializeField] private Image blinkImage; // UI Image that covers the screen (should be initially transparent).
    [SerializeField] private float blinkInDuration = 0.05f;
    [SerializeField] private float blinkOutDuration = 0.1f;
    [SerializeField] private Color blinkColor = new Color(1f, 1f, 1f, 0.3f); // A subtle white flash.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (blinkImage == null)
        {
            Debug.LogError("ScreenBlinkEffect: Blink Image not assigned in the Inspector!");
        }
        else
        {
            blinkImage.color = new Color(blinkImage.color.r, blinkImage.color.g, blinkImage.color.b, 0f);
        }
    }

    // Call this method to trigger a screen blink.
    public void Blink()
    {
        if (blinkImage != null)
        {
            StartCoroutine(DoBlink());
        }
    }

    private IEnumerator DoBlink()
    {
        // Force the original color to be transparent.
        Color originalColor = new Color(blinkImage.color.r, blinkImage.color.g, blinkImage.color.b, 0f);
        float timer = 0f;
    
        // Fade in quickly.
        while (timer < blinkInDuration)
        {
            timer += Time.deltaTime;
            blinkImage.color = Color.Lerp(originalColor, blinkColor, timer / blinkInDuration);
            yield return null;
        }
    
        timer = 0f;
        // Fade out back to original.
        while (timer < blinkOutDuration)
        {
            timer += Time.deltaTime;
            blinkImage.color = Color.Lerp(blinkColor, originalColor, timer / blinkOutDuration);
            yield return null;
        }
        blinkImage.color = originalColor;
    }
}