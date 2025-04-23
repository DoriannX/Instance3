using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.1f;  // Duration of the flash.

    private Material[] originalMaterials; // Array to store the original materials of the renderers.
    private Color[] originalColors; // Array to store the original colors of the materials.

    private void Awake()
    {
        List<Material> materialsList = new();
        List<Color> colorsList = new();

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    materialsList.Add(mat);
                    colorsList.Add(mat.color);
                }
            }
        }

        originalMaterials = materialsList.ToArray();
        originalColors = colorsList.ToArray();
    }

    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        // Flash rouge
        foreach (Material mat in originalMaterials)
        {
            mat.color = Color.red;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restaure les couleurs
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            originalMaterials[i].color = originalColors[i];
        }   
    }
}
