using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using Cloth.Springs;
using UnityEngine;

namespace Cloth.Technique
{
    public class MassSpring
    {
        private readonly List<(int, int, int)> _triangles;
        private readonly Vector3[] _vertices;
        private List<SpringPair> _stretchSpringPairs;
        private List<SpringPair> _shearSpringPairs;

        public MassSpring(int[] triangles, Vector3[] vertices)
        {
            _triangles = Triangle.GetTrianglesFromFlatList(triangles.ToList()).ToList();;
            _vertices = vertices;

            CreateSpringPairings();
        }

        private void CreateSpringPairings()
        {
            _stretchSpringPairs = SpringPairCreator.CreateStretchSprings(_triangles, _vertices);
            _shearSpringPairs = SpringPairCreator.GetShearSprings(_triangles, _vertices);
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
            
            foreach (var pair in _shearSpringPairs)
            {
                Debug.DrawLine(_vertices[pair.firstIndex], _vertices[pair.secondIndex], Color.blue);
            }
        }

        #endregion
    }
}