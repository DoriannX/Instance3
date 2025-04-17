using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBorderFlash : MonoBehaviour
{
    public static ScreenBorderFlash Instance { get; private set; }

    [SerializeField] private Image borderImage; // Assign a UI Image set up as a border overlay.
    [SerializeField] private float flashInDuration = 0.1f;
    [SerializeField] private float flashOutDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.5f); // Red, semi-transparent.

    private void Awake()
    {
        // Singleton setup.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (borderImage == null)
        {
            Debug.LogError("ScreenBorderFlash: Border Image is not assigned in the Inspector!");
        }
    }

    // Call this method (e.g., from the player's damage system) to trigger the border flash.
    public void FlashBorder()
    {
        if(borderImage != null)
            StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        Color initialColor = borderImage.color;
        float timer = 0f;

        // Fade in to the flashColor.
        while (timer < flashInDuration)
        {
            timer += Time.deltaTime;
            borderImage.color = Color.Lerp(initialColor, flashColor, timer / flashInDuration);
            yield return null;
        }

        timer = 0f;
        // Fade out back to the initial color (assumed to be transparent).
        while (timer < flashOutDuration)
        {
            timer += Time.deltaTime;
            borderImage.color = Color.Lerp(flashColor, initialColor, timer / flashOutDuration);
            yield return null;
        }
        borderImage.color = initialColor;
    }
} 