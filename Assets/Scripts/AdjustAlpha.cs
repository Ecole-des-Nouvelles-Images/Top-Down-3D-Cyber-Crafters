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
        // Récupère tous les MeshRenderer des enfants du GameObject
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        // Parcourt chaque MeshRenderer pour ajuster l'alpha de son matériau
        foreach (MeshRenderer renderer in renderers)
        {
            Material material = renderer.material;

            if (material.HasProperty("_Color"))
            {
                Color color = material.color;
                color.a = alphaValue;
                material.color = color;

                // Assurez-vous que le mode de rendu est correct pour la transparence
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