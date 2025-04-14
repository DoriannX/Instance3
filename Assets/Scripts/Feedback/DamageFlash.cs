using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Material damageMaterial; // The material to apply for the flash effect.
    [SerializeField] private float flashDuration = 0.1f;  // Duration of the flash.

    private Renderer[] renderers;
    private Material[] originalMaterials;

    private void Awake()
    {
        // Get all renderers on this object or its children.
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0)
        {
            Renderer rootRenderer = GetComponent<Renderer>();
            if (rootRenderer != null)
            {
                renderers = new Renderer[] { rootRenderer };
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} has no Renderer components. Damage flash disabled.");
                renderers = new Renderer[0];
            }
        }
        
        // Cache each renderer’s original material.
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    // Call this method to perform the flash effect.
    public void Flash()
    {
        if (damageMaterial == null)
        {
            Debug.LogError($"{gameObject.name}: Damage material is not assigned for flashing!");
            return;
        }
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        // Swap in the damage material.
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = damageMaterial;
        }
        
        yield return new WaitForSeconds(flashDuration);
        
        // Restore the original materials.
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}