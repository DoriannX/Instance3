using System.Collections;
using UnityEngine;

public class DashGhostEffect : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostLifetime = 0.3f;

    // Call this method to trigger a ghost effect.
    public void TriggerGhost()
    {
        if (ghostPrefab != null)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            
            StartCoroutine(FadeAndDestroyGhost(ghost, ghostLifetime));
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No ghost prefab assigned for dash effect.");
        }
    }

    private IEnumerator FadeAndDestroyGhost(GameObject ghost, float duration)
    {
        Renderer ghostRenderer = ghost.GetComponentInChildren<Renderer>();
        
        // Check if the ghost has a renderer to fade.
        if (ghostRenderer == null)
        {
            yield return new WaitForSeconds(duration);
            Destroy(ghost);
            yield break;
        }

        // Get the instance of the ghost material
        Material ghostMat = ghostRenderer.material;
        Color originalColor = ghostMat.color;
        float timer = 0f;

        // Gradually fade the alpha from its original value to zero.
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(originalColor.a, 0f, timer / duration);
            Color newColor = originalColor;
            newColor.a = newAlpha;
            ghostMat.color = newColor;
            yield return null;
        }
        // Ensure the ghost ends fully transparent and then destroy it.
        ghostMat.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(ghost);
    }
}
