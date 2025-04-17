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
        
        // Ensure the border image starts fully transparent.
        var c = borderImage.color;
        c.a = 0f;
        borderImage.color = c;
    }

    // Call this method (e.g., from the player's damage system) to trigger the border flash.
    public void FlashBorder()
    {
        if (borderImage == null) return;
        StopAllCoroutines();        // cancels any in‑flight DoFlash
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        // 1) Fade in from transparent (0) to flashColor.a
        float timer = 0f;
        while (timer < flashInDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, flashColor.a, timer / flashInDuration);
            borderImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // 2) Fade back out to fully transparent
        timer = 0f;
        while (timer < flashOutDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(flashColor.a, 0f, timer / flashOutDuration);
            borderImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // 3) Guarantee a final reset
        borderImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }

} 