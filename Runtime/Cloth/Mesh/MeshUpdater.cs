using System.Linq;
using UnityEngine;

namespace Cloth.Mesh
{
    public static class MeshUpdater
    {
        public static void UpdateMeshes(MeshFilter meshFilter, MeshCollider meshCollider, Vector3[] newPositions)
        {
            var localPositions = newPositions.Select(meshFilter.gameObject.transform.InverseTransformPoint).ToArray();
            var meshToUpdate = meshFilter.sharedMesh;
            
            meshToUpdate.vertices = localPositions;
            meshToUpdate.RecalculateBounds();
            meshToUpdate.RecalculateNormals();

            // Need to assign the mesh every frame to get intersections happening correctly.
            // See: https://forum.unity.com/threads/how-to-update-a-mesh-collider.32467/
            if (meshCollider != null)
            {
                meshCollider.sharedMesh = meshToUpdate;
            }
        }
    }
}