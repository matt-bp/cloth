using System;
using Cloth.Technique;
using UnityEngine;

namespace Cloth.Behaviour
{
    [AddComponentMenu("Matt/Cloth")]
    [RequireComponent(typeof(MeshFilter))]
    public class Cloth : MonoBehaviour
    {

        private MassSpring _massSpring;
        
        private void Start()
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;

            _massSpring = new MassSpring(mesh.triangles, mesh.vertices);
        }

        private void OnDrawGizmos()
        {
            _massSpring?.OnDrawGizmos();
        }
    }
}