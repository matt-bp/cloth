using System;
using System.Linq;
using Cloth.Mesh;
using Cloth.Provider;
using Cloth.Springs;
using Cloth.Technique;
using UnityEngine;

namespace Cloth.Behaviour
{
    [AddComponentMenu("Matt/Cloth")]
    [RequireComponent(typeof(MeshFilter))]
    public class Cloth : MonoBehaviour
    {
        [Header("Simulation Parameters")] [SerializeField]
        private float k;

        [SerializeField] private float kd;
        [SerializeField] private float surfaceDensity;
        [SerializeField] private Vector3 gravity;
        [SerializeField] private bool doSimulation;
        
        private MassSpring _massSpring;
        private MeshFilter _meshFilter;

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            var mesh = _meshFilter.sharedMesh;
            var vertices = mesh.vertices.Select(_meshFilter.gameObject.transform.TransformPoint).ToArray();

            var massProvider = new MassProvider(surfaceDensity);
            var springProvider = new SpringProvider();
            _massSpring = new MassSpring(massProvider, springProvider, mesh.triangles, vertices, k, kd);
        }

        private void FixedUpdate()
        {
            if (!doSimulation) return;

            // Calculate external forces
            var externalForces = _massSpring.Masses.Select(m => gravity * m).ToArray();

            _massSpring.Step(Time.fixedTime, externalForces);

            MeshUpdater.UpdateMeshes(_meshFilter, null, _massSpring.Positions);
        }

        private void OnDrawGizmos()
        {
            _massSpring?.OnDrawGizmos();
        }
    }
}