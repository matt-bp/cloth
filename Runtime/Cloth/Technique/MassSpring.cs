using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Technique
{
    public class MassSpring
    {
        private readonly int[] _triangles;
        private readonly Vector3[] _vertices;
        private SpringPair[] _stretchSpringPairs;

        public MassSpring(int[] triangles, Vector3[] vertices)
        {
            // Connect vertices with springs
            _triangles = triangles;
            _vertices = vertices;

            CreateSpringPairings();
        }

        private void CreateSpringPairings()
        {
            _stretchSpringPairs = new SpringPair[] { new() { firstIndex = 0, secondIndex = 1 } };
        }

        private void ComputePairForces(int first, int second)
        {
        }

        private void GetSpringForce()
        {
        }

        #region Gizmos

        public void OnDrawGizmos()
        {
            foreach (var pair in _stretchSpringPairs)
            {
                Debug.DrawLine(_vertices[pair.firstIndex], _vertices[pair.secondIndex], Color.magenta);
            }
        }

        #endregion
    }
}