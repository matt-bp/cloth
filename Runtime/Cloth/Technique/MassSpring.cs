using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using Cloth.Provider;
using Cloth.Springs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Cloth.Technique
{
    public class MassSpring
    {
        public float StretchK = 60;
        public float StretchKd = 10;
        public float ShearK = 60;
        public float ShearKd = 10;
        public float BendK = 10;
        public float BendKd = 5;

        public Vector3[] Positions { get; }

        private Vector3[] _velocities;
        private Vector3[] _forces;
        private readonly List<SpringPair> _stretchSpringPairs;
        private readonly List<SpringPair> _shearSpringPairs;
        private readonly List<SpringPair> _bendSpringPairs;
        public float[] Masses { get; }

        public List<int> ConstrainedIndices = new();
        private bool IsAnchor(int index) => ConstrainedIndices.Contains(index);

        public MassSpring(IMassProvider massProvider, ISpringProvider springProvider, int[] triangles,
            Vector3[] vertices)
        {
            var groupedTriangles = Triangle.GetTrianglesFromFlatList(triangles.ToList()).ToArray();

            Positions = vertices;
            _velocities = new Vector3[vertices.Length];
            _forces = new Vector3[vertices.Length];
            Masses = massProvider.GetMasses(groupedTriangles, Positions);
            _stretchSpringPairs = springProvider.CreateStretchSprings(groupedTriangles, Positions);
            _shearSpringPairs = springProvider.CreateShearSprings(groupedTriangles, Positions);
            _bendSpringPairs = springProvider.CreateBendSprings(groupedTriangles, Positions);
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
            _forces = new Vector3[_forces.Length];;
        }

        /// <summary>
        /// This is used in the cloth relaxation mode to only have each frames internal cloth forces move the cloth.
        /// </summary>
        public void ResetVelocities()
        {
            _velocities = new Vector3[_velocities.Length];
        }

        private void ComputeForces()
        {
            foreach (var pair in _stretchSpringPairs)
            {
                ComputeForceForPair(pair.FirstIndex, pair.SecondIndex, pair.RestLength, StretchK, StretchKd);
            }

            foreach (var pair in _shearSpringPairs)
            {
                ComputeForceForPair(pair.FirstIndex, pair.SecondIndex, pair.RestLength, ShearK, ShearKd);
            }

            foreach (var pair in _bendSpringPairs)
            {
                ComputeForceForPair(pair.FirstIndex, pair.SecondIndex, pair.RestLength, BendK, BendKd);
            }
        }

        private void ComputeForceForPair(int first, int second, float restLength, float k, float kd)
        {
            var springForce = Forces.GetSpringForce(Positions[first], Positions[second], k, restLength);

            var dampingForce = Forces.GetDampingForce(_velocities[first], _velocities[second], kd);

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

            foreach (var pair in _bendSpringPairs)
            {
                Debug.DrawLine(Positions[pair.FirstIndex], Positions[pair.SecondIndex], Color.yellow);
            }
        }

        #endregion
    }
}