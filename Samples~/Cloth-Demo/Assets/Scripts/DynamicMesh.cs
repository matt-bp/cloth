using UnityEngine;

public class DynamicMesh : MonoBehaviour
{
    [SerializeField] private Vector3[] newVertices;
    [SerializeField] private int[] newTriangles;

    private void Awake()
    {
        var meshFilter = GetComponent<MeshFilter>();
            
        var mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}