using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using Cloth.Springs;
using UnityEngine;

namespace Cloth.Technique
{
    public class MassSpring
    {
        private readonly float _k;
        private readonly float _kd;

        private readonly List<(int, int, int)> _triangles;
        public Vector3[] Positions { get; }

        private readonly Vector3[] _velocities;
        private Vector3[] _forces;
        private List<SpringPair> _stretchSpringPairs;
        private List<SpringPair> _shearSpringPairs;
        public float[] Masses { get; }

        private List<int> _constrainedIndices = new();
        private bool IsAnchor(int index) => _constrainedIndices.Contains(index);


        public MassSpring(int[] triangles, Vector3[] vertices, float k, float kd, float surfaceDensity)
        {
            _k = k;
            _kd = kd;

            _triangles = Triangle.GetTrianglesFromFlatList(triangles.ToList()).ToList();
            Positions = vertices;
            _velocities = new Vector3[vertices.Length];
            _forces = new Vector3[vertices.Length];
            Masses = DataStructures.Cloth.GetMasses(_triangles, Positions, surfaceDensity);

            _stretchSpringPairs = SpringPairCreator.CreateStretchSprings(_triangles, Positions);
            _shearSpringPairs = SpringPairCreator.GetShearSprings(_triangles, Positions);
        }

        public void Step(float dt, Vector3[] externalForces)
        {
            // ComputeForces();
            // Pickup here, not working
            _forces = _forces.Select(x => Vector3.zero).ToArray();

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

        private void ComputeForces()
        {
            _forces = _forces.Select(x => Vector3.zero).ToArray();

            foreach (var pair in _stretchSpringPairs)
            {
                ComputeForceForPair(pair.firstIndex, pair.secondIndex, pair.restLength);
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
                Debug.DrawLine(Positions[pair.firstIndex], Positions[pair.secondIndex], Color.magenta);
            }

            foreach (var pair in _shearSpringPairs)
            {
                Debug.DrawLine(Positions[pair.firstIndex], Positions[pair.secondIndex], Color.blue);
            }
        }

        #endregion
    }
}