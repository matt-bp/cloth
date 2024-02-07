using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using Cloth.Springs;
using UnityEngine;

namespace Cloth.Behaviour
{
[RequireComponent(typeof(MeshFilter))]
    public class MassSpringCloth : MonoBehaviour
    {
        #region Simulation Constants

        private const int K = 4;
        private const float Kd = 1.0f;
        private const float Gravity = -10.0f;

        #endregion

        private MeshFilter meshFilter;
        
        [SerializeField] private Vector3[] initialPositions;
        [SerializeField] private Vector3[] positions;
        [SerializeField] private Vector3[] velocities;
        [SerializeField] private Vector3[] forces;
        [SerializeField] private List<int> constrainedIndices = new();
        [SerializeField] private List<SpringPair> springs = new();
        [SerializeField] private float[] masses;
        [SerializeField] private float surfaceDensity;
        [SerializeField] private bool doUpdate;
        
        private void Start()
        {
            meshFilter = GetComponent<MeshFilter>();

            initialPositions = meshFilter.mesh.vertices.Select(v => meshFilter.gameObject.transform.TransformPoint(v))
                .ToArray();

            positions = initialPositions.Select(v => v).ToArray();
            velocities = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
            forces = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();

            InitializeMasses();
        }

        private void InitializeMasses()
        {
            var totalArea = meshFilter.mesh.triangles.Select((value, i) => new { value, i })
                .GroupBy(v => v.i / 3)
                .Select(g => g.Select(x => x.value).ToList())
                .Sum(v => Triangle.GetArea(positions[v[0]], positions[v[1]], positions[v[2]]));

            var totalMass = surfaceDensity * totalArea;
            
            masses = Enumerable.Range(0, positions.Length).Select(_ => totalMass / positions.Length).ToArray();
        }

        private void FixedUpdate()
        {
            if (!doUpdate) return;  
            // Assert.IsTrue(externalForces.Length == forces.Length);

            SimulationStep(Time.fixedTime, Array.Empty<Vector3>());

            SetMeshVertices(positions);
        }

        private void SetMeshVertices(Vector3[] newPositions)
        {
            meshFilter.mesh.vertices =
                newPositions.Select(v => meshFilter.gameObject.transform.InverseTransformPoint(v)).ToArray();
            meshFilter.mesh.RecalculateBounds();
        }

        public Vector3[] Positions => positions;

        public void Reset()
        {
            positions = initialPositions.Select(v => v).ToArray();
            SetMeshVertices(initialPositions);
            velocities = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
            forces = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
        }

        private void SimulationStep(float dt, Vector3[] externalForces)
        {
            ComputeForces();

            for (var i = 0; i < externalForces.Length; i++)
            {
                if (IsAnchor(i)) continue;
                
                forces[i] += externalForces[i];
            }

            // Update velocities * positions
            for (var i = 0; i < forces.Length; i++)
            {
                var acceleration = forces[i] / masses[i];
                velocities[i] += acceleration * dt;
                positions[i] += velocities[i] * dt;
            }
        }

        private bool IsAnchor(int index)
        {
            return constrainedIndices.Contains(index);
        }

        private void ComputeForces()
        {
            forces = forces.Select(x => Vector3.zero).ToArray();

            // Set gravity contribution
            for (var i = 0; i < forces.Length; i++)
            {
                if (IsAnchor(i)) continue;

                forces[i].y = Gravity * masses[i];
            }

            foreach (var pair in springs)
            {
                ComputeForceForPair(pair.FirstIndex, pair.SecondIndex, pair.RestLength);
            }
        }

        private void ComputeForceForPair(int first, int second, float restLength)
        {
            var springForce = GetSpringForce(positions[first], positions[second], restLength);

            var dampingForce = GetDampingForce(velocities[first], velocities[second]);

            if (!IsAnchor(first))
            {
                forces[first] -= springForce - dampingForce;
            }

            if (!IsAnchor(second))
            {
                forces[second] += springForce - dampingForce;
            }
        }

        private Vector3 GetSpringForce(Vector3 position1, Vector3 position2, float restLength)
        {
            var distance = Vector3.Distance(position1, position2);
            var force = K * (distance - restLength) * ((position1 - position2) / distance);
            return force;
        }

        private Vector3 GetDampingForce(Vector3 velocity1, Vector3 velocity2)
        {
            return -Kd * (velocity1 - velocity2);
        }
    }
}