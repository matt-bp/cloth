using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Springs
{
    public interface ISpringProvider
    {
        List<SpringPair> CreateStretchSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices);
        public List<SpringPair> CreateShearSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices);
    }
    
    public class SpringProvider : ISpringProvider
    {
        public List<SpringPair> CreateStretchSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices)
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
                        { FirstIndex = triangle.Item2, SecondIndex = triangle.Item3, RestLength = edge2 });
                    pairs.Add(new SpringPair
                        { FirstIndex = triangle.Item3, SecondIndex = triangle.Item1, RestLength = edge3 });
                }
                else if (edge2 > edge1 && edge2 > edge3)
                {
                    pairs.Add(new SpringPair
                        { FirstIndex = triangle.Item3, SecondIndex = triangle.Item1, RestLength = edge3 });
                    pairs.Add(new SpringPair
                        { FirstIndex = triangle.Item1, SecondIndex = triangle.Item2, RestLength = edge1 });
                }
                else
                {
                    pairs.Add(new SpringPair
                        { FirstIndex = triangle.Item1, SecondIndex = triangle.Item2, RestLength = edge1 });
                    pairs.Add(new SpringPair
                        { FirstIndex = triangle.Item2, SecondIndex = triangle.Item3, RestLength = edge2 });
                }
            }

            return pairs;
        }

        public List<SpringPair> CreateShearSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices)
        {
            var pairs = new List<SpringPair>();

            var trianglePairs = Quad.MakeFromSharedEdges(triangles.ToList());

            Debug.Log($"Num pairs: {trianglePairs.Count}");

            foreach (var pair in trianglePairs)
            {
                var sharedEdge = (vertices[pair.Item2] - vertices[pair.Item3]).magnitude;
                var oppositeEdge = (vertices[pair.Item1] - vertices[pair.Item4]).magnitude;

                if (Math.Abs(sharedEdge - oppositeEdge) < 0.000001f)
                {
                    pairs.Add(new SpringPair
                        { FirstIndex = pair.Item2, SecondIndex = pair.Item3, RestLength = sharedEdge });
                    pairs.Add(new SpringPair
                        { FirstIndex = pair.Item1, SecondIndex = pair.Item4, RestLength = oppositeEdge });
                }
            }

            return pairs;
        }

        public List<SpringPair> CreateBendSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices)
        {
            var pairs = new List<SpringPair>();

            return pairs; 
        }
    }
}