using System;
using UnityEngine;

public class AdjustAlpha : MonoBehaviour
{
    [Range(0, 1)]
    public float alpha = 0.5f;

    private void Start()
    {
        SetAlpha(alpha);
    }

    private void SetAlpha(float alphaValue)
    {
        // Get the MeshRenderer of the current GameObject
        MeshRenderer selfRenderer = GetComponent<MeshRenderer>();

        // Get all the MeshRenderers from the children of the GameObject
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

        // Combine selfRenderer and childRenderers into one array
        MeshRenderer[] allRenderers = new MeshRenderer[childRenderers.Length + 1];
        allRenderers[0] = selfRenderer;
        Array.Copy(childRenderers, 0, allRenderers, 1, childRenderers.Length);

        // Go through each MeshRenderer to adjust the alpha of its material
        foreach (MeshRenderer renderer in allRenderers)
        {
            if (renderer == null) continue; // Skip if renderer is null

            Material material = renderer.material;

            if (material.HasProperty("_Color"))
            {
                Color color = material.color;
                color.a = alphaValue;
                material.color = color;

                // Make sure the rendering mode is correct for transparency
                material.SetFloat("_Mode", 2);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
        }
    }
}