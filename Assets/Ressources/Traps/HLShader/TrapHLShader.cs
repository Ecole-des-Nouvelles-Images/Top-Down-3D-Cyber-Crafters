using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHLShader : MonoBehaviour

{
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer trapRenderer;

    void Start()
    {
        trapRenderer = GetComponent<Renderer>();
        originalMaterial = trapRenderer.material;
    }

    public void ActivateTrap()
    {
        StartCoroutine(HighlightTrap());
        // Ajoutez ici le code pour activer le piège
    }

    private IEnumerator HighlightTrap()
    {
        trapRenderer.material = highlightMaterial;
        yield return new WaitForSeconds(1);
        trapRenderer.material = originalMaterial;
    }
}
