using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using Cloth.Provider;
using Cloth.Springs;
using UnityEngine;

namespace Cloth.Technique
{
    public class MassSpring
    {
        private readonly float _k;
        private readonly float _kd;

        public Vector3[] Positions { get; }

        private readonly Vector3[] _velocities;
        private Vector3[] _forces;
        private readonly List<SpringPair> _stretchSpringPairs;
        private readonly List<SpringPair> _shearSpringPairs;
        public float[] Masses { get; }

        public readonly List<int> ConstrainedIndices = new();
        private bool IsAnchor(int index) => ConstrainedIndices.Contains(index);

        public MassSpring(IMassProvider massProvider, ISpringProvider springProvider, int[] triangles, Vector3[] vertices, float k, float kd)
        {
            _k = k;
            _kd = kd;

            var groupedTriangles = Triangle.GetTrianglesFromFlatList(triangles.ToList()).ToArray();
            
            Positions = vertices;
            _velocities = new Vector3[vertices.Length];
            _forces = new Vector3[vertices.Length];
            Masses = massProvider.GetMasses(groupedTriangles, Positions);
            _stretchSpringPairs = springProvider.CreateStretchSprings(groupedTriangles, Positions).Take(1).ToList();
            _shearSpringPairs = springProvider.CreateShearSprings(groupedTriangles, Positions).Take(0).ToList();
        }

        public void Step(float dt, Vector3[] externalForces)
        {
            ResetForces();
            
            ComputeForces();

            for (var i = 0; i < externalForces.Length; i++)
            {
                if (IsAnchor(i)) continue;

                _forces[i] += externalForces[i];
            }

            // Update velocities & positions
            for (var i = 0; i < _forces.Length; i++)
            {
                var acceleration = _forces[i] / Masses[i];
                _velocities[i] += acceleration * dt;
                Positions[i] += _velocities[i] * dt;
            }
        }

        private void ResetForces()
        {
            _forces = _forces.Select(x => Vector3.zero).ToArray();
        }

        private void ComputeForces()
        {
            foreach (var pair in _stretchSpringPairs)
            {
                ComputeForceForPair(pair.FirstIndex, pair.SecondIndex, pair.RestLength);
            }
        }
        
        private void ComputeForceForPair(int first, int second, float restLength)
        {
            var springForce = Forces.GetSpringForce(Positions[first], Positions[second], _k, restLength);

            var dampingForce = Forces.GetDampingForce(_velocities[first], _velocities[second], _kd);

            if (!IsAnchor(first))
            {
                _forces[first] -= springForce - dampingForce;
            }

            if (!IsAnchor(second))
            {
                _forces[second] += springForce - dampingForce;
            }
        }

        #region Gizmos

        public void OnDrawGizmos()
        {
            foreach (var pair in _stretchSpringPairs)
            {
                Debug.DrawLine(Positions[pair.FirstIndex], Positions[pair.SecondIndex], Color.magenta);
            }

            foreach (var pair in _shearSpringPairs)
            {
                Debug.DrawLine(Positions[pair.FirstIndex], Positions[pair.SecondIndex], Color.blue);
            }
        }

        #endregion
    }
}