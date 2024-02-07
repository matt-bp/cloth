using System;
using System.Collections.Generic;
using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Springs
{
    public static class SpringPairCreator
    {
        public static List<SpringPair> CreateStretchSprings(List<(int, int, int)> triangles, Vector3[] vertices)
        {
            var pairs = new List<SpringPair>();

            foreach (var triangle in triangles)
            {
                var v1 = vertices[triangle.Item1];
                var v2 = vertices[triangle.Item2];
                var v3 = vertices[triangle.Item3];

                var edge1 = (v2 - v1).magnitude;
                var edge2 = (v3 - v2).magnitude;
                var edge3 = (v1 - v3).magnitude;

                if (edge1 > edge2 && edge1 > edge3)
                {
                    pairs.Add(new SpringPair
                        { firstIndex = triangle.Item2, secondIndex = triangle.Item3, restLength = edge2 });
                    pairs.Add(new SpringPair
                        { firstIndex = triangle.Item3, secondIndex = triangle.Item1, restLength = edge3 });
                }
                else if (edge2 > edge1 && edge2 > edge3)
                {
                    pairs.Add(new SpringPair
                        { firstIndex = triangle.Item3, secondIndex = triangle.Item1, restLength = edge3 });
                    pairs.Add(new SpringPair
                        { firstIndex = triangle.Item1, secondIndex = triangle.Item2, restLength = edge1 });
                }
                else
                {
                    pairs.Add(new SpringPair
                        { firstIndex = triangle.Item1, secondIndex = triangle.Item2, restLength = edge1 });
                    pairs.Add(new SpringPair
                        { firstIndex = triangle.Item2, secondIndex = triangle.Item3, restLength = edge2 });
                }
            }

            return pairs;
        }

        public static List<SpringPair> GetShearSprings(List<(int, int, int)> triangles, Vector3[] vertices)
        {
            var pairs = new List<SpringPair>();

            var trianglePairs = TrianglePair.MakeFromSharedEdges(triangles);

            Debug.Log($"Num pairs: {trianglePairs.Count}");

            foreach (var pair in trianglePairs)
            {
                var sharedEdge = (vertices[pair.Item2] - vertices[pair.Item3]).magnitude;
                var oppositeEdge = (vertices[pair.Item1] - vertices[pair.Item4]).magnitude;

                if (Math.Abs(sharedEdge - oppositeEdge) < 0.000001f)
                {
                    pairs.Add(new SpringPair
                        { firstIndex = pair.Item2, secondIndex = pair.Item3, restLength = sharedEdge });
                    pairs.Add(new SpringPair
                        { firstIndex = pair.Item1, secondIndex = pair.Item4, restLength = oppositeEdge });
                }
            }

            return pairs;
        }
    }
}