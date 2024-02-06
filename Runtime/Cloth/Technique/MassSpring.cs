using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Technique
{
    public class MassSpring
    {
        private readonly List<(int, int, int)> _triangles;
        private readonly Vector3[] _vertices;
        private SpringPair[] _stretchSpringPairs;

        public MassSpring(int[] triangles, Vector3[] vertices)
        {
            _triangles = Triangle.GetTrianglesFromFlatList(triangles.ToList()).ToList();;
            _vertices = vertices;

            CreateSpringPairings();
        }

        private void CreateSpringPairings()
        {
            CreateStretchPairings();
        }

        private void CreateStretchPairings()
        {
            var pairs = new List<SpringPair>();
            
            foreach (var triangle in _triangles)
            {
                var v1 = _vertices[triangle.Item1];
                var v2 = _vertices[triangle.Item2];
                var v3 = _vertices[triangle.Item3];

                var v2v1 = (v2 - v1).sqrMagnitude;
                var v3v2 = (v3 - v2).sqrMagnitude;
                var v1v3 = (v1 - v3).sqrMagnitude;

                if (v2v1 > v3v2 && v2v1 > v1v3)
                {
                    pairs.Add(new SpringPair { firstIndex = triangle.Item2, secondIndex = triangle.Item3 });
                    pairs.Add(new SpringPair { firstIndex = triangle.Item3, secondIndex = triangle.Item1 });
                }
                else if (v3v2 > v2v1 && v3v2 > v1v3)
                {
                    pairs.Add(new SpringPair { firstIndex = triangle.Item3, secondIndex = triangle.Item1 });
                    pairs.Add(new SpringPair { firstIndex = triangle.Item1, secondIndex = triangle.Item2 });
                }
                else
                {
                    pairs.Add(new SpringPair { firstIndex = triangle.Item1, secondIndex = triangle.Item2 });
                    pairs.Add(new SpringPair { firstIndex = triangle.Item2, secondIndex = triangle.Item3 });
                }
            }

            _stretchSpringPairs = pairs.ToArray();
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