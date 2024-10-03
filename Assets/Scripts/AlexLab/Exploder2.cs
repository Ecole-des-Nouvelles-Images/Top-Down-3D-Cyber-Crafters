using UnityEngine;
using System.Collections.Generic;

public class ExplodeObject : MonoBehaviour
{
    public GameObject originalObject; // L'objet original à découper
    public int numberOfPieces = 10; // Nombre de morceaux à créer
    public float explosionForce = 1000f; // Force de l'explosion
    public float explosionRadius = 5f; // Rayon de l'explosion
    public float thickness = 0.1f; // Épaisseur des morceaux

    void Start()
    {
        Explode();
    }

    void Explode()
    {
        if (originalObject == null)
        {
            Debug.LogError("Original object not assigned.");
            return;
        }

        // Désactive l'objet original
        originalObject.SetActive(false);

        // Récupère le maillage de l'objet original
        Mesh originalMesh = originalObject.GetComponent<MeshFilter>().mesh;

        // Crée un tableau de morceaux
        List<GameObject> pieces = new List<GameObject>();

        // Découpe le maillage en morceaux
        for (int i = 0; i < numberOfPieces; i++)
        {
            GameObject piece = new GameObject("Piece " + i);
            piece.transform.position = originalObject.transform.position;
            piece.transform.rotation = originalObject.transform.rotation;

            Mesh pieceMesh = new Mesh();
            pieceMesh.vertices = originalMesh.vertices;
            pieceMesh.triangles = GetRandomTriangles(originalMesh.triangles, originalMesh.triangles.Length / numberOfPieces);
            pieceMesh.RecalculateNormals();

            // Ajoute de l'épaisseur aux morceaux
            pieceMesh = AddThickness(pieceMesh, thickness);

            MeshFilter meshFilter = piece.AddComponent<MeshFilter>();
            meshFilter.mesh = pieceMesh;

            MeshRenderer meshRenderer = piece.AddComponent<MeshRenderer>();
            meshRenderer.material = originalObject.GetComponent<MeshRenderer>().material;

            Rigidbody rb = piece.AddComponent<Rigidbody>();
            rb.mass = 1f;

            // Applique une force d'explosion à chaque morceau
            rb.AddExplosionForce(explosionForce, originalObject.transform.position, explosionRadius);

            pieces.Add(piece);
        }

        // Désactive l'objet original
        originalObject.SetActive(false);
    }

    int[] GetRandomTriangles(int[] originalTriangles, int numberOfTriangles)
    {
        List<int> randomTriangles = new List<int>();
        List<int> indices = new List<int>(originalTriangles);

        for (int i = 0; i < numberOfTriangles * 3; i++)
        {
            int index = Random.Range(0, indices.Count);
            randomTriangles.Add(indices[index]);
            indices.RemoveAt(index);
        }

        return randomTriangles.ToArray();
    }

    Mesh AddThickness(Mesh mesh, float thickness)
    {
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);
        List<Vector3> normals = new List<Vector3>(mesh.normals);

        int vertexCount = vertices.Count;

        for (int i = 0; i < vertexCount; i++)
        {
            vertices.Add(vertices[i] + normals[i] * thickness);
        }

        for (int i = 0; i < triangles.Count; i += 3)
        {
            int v1 = triangles[i];
            int v2 = triangles[i + 1];
            int v3 = triangles[i + 2];

            triangles.Add(v3 + vertexCount);
            triangles.Add(v2 + vertexCount);
            triangles.Add(v1 + vertexCount);

            triangles.Add(v1);
            triangles.Add(v1 + vertexCount);
            triangles.Add(v2 + vertexCount);

            triangles.Add(v2);
            triangles.Add(v2 + vertexCount);
            triangles.Add(v3 + vertexCount);

            triangles.Add(v3);
            triangles.Add(v3 + vertexCount);
            triangles.Add(v1 + vertexCount);
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = triangles.ToArray();
        newMesh.RecalculateNormals();

        return newMesh;
    }
}
