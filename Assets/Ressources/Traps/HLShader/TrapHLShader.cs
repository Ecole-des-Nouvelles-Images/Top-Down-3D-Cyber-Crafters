using System.Collections;
using UnityEngine;

public class TrapHLShader : MonoBehaviour

{
    // public Material highlightMaterial;
    // private Material originalMaterial;
    private Renderer _trapRenderer;
    private Color _originalColor;
    //private Color _playerColor;

    void Start()
    {
        _trapRenderer = GetComponent<Renderer>();
        // originalMaterial = trapRenderer.material;
    }

    public void ActivateTrap(Color playerColor)
    {
        StartCoroutine(HighlightTrap(playerColor));
        // Ajoutez ici le code pour activer le piège
    }

    private IEnumerator HighlightTrap(Color playerColor)
    {
        var material = _trapRenderer.material;
        _originalColor = material.color;
        material.color = playerColor;
        yield return new WaitForSeconds(1);
        material.color = _originalColor;
    }
}
