using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using Cloth.Provider;
using UnityEngine;

namespace Cloth.Springs
{
    public interface ISpringProvider
    {
        List<SpringPair> CreateStretchSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices);
        public List<SpringPair> CreateShearSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices);
        public List<SpringPair> CreateBendSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices);
    }

    public class SpringProvider : ISpringProvider
    {
        public List<SpringPair> CreateStretchSprings(IEnumerable<(int, int, int)> triangles, Vector3[] vertices)
        {
            return triangles
                .Select(t => DirectedEdgeProvider.GetEdgesFromTriangle(
                    t,
                    vertices[t.Item1],
                    vertices[t.Item2],
                    vertices[t.Item3]))
                .SelectMany(dr => dr)
                .Select(d => new SpringPair
                {
                    FirstIndex = d.Start,
                    SecondIndex = d.End,
                    RestLength = d.Edge.magnitude
                }).ToList();
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

            var directedEdges = triangles
                .Select(t =>
                    DirectedEdgeProvider.GetEdgesFromTriangle(t, vertices[t.Item1], vertices[t.Item2],
                        vertices[t.Item3]))
                .Select(l => l.DuplicateAndReverse())
                .SelectMany(l => l)
                .ToList();

            var groupedEdges = new Dictionary<int, List<DirectedEdge>>();

            foreach (var edge in directedEdges)
            {
                if (!groupedEdges.ContainsKey(edge.Start))
                {
                    groupedEdges.Add(edge.Start, new List<DirectedEdge>());
                }

                if (groupedEdges[edge.Start].Any(e => e.Start == edge.Start && e.End == edge.End))
                {
                    continue;
                }

                groupedEdges[edge.Start].Add(edge);
            }

            foreach (var (index, edges) in groupedEdges)
            {
                foreach (var edge in edges)
                {
                    foreach (var endEdge in groupedEdges[edge.End])
                    {
                        if (pairs.Any(p => p.FirstIndex == endEdge.End && p.SecondIndex == index))
                        {
                            continue;
                        }

                        var matchesDirection =
                            Math.Abs(Vector3.Dot(edge.Edge.normalized, endEdge.Edge.normalized) - 1.0f) < float.Epsilon;

                        if (!matchesDirection) continue;

                        var restLength = edge.Edge.magnitude + endEdge.Edge.magnitude;

                        pairs.Add(new SpringPair
                        {
                            FirstIndex = index, SecondIndex = endEdge.End, RestLength = restLength
                        });
                    }
                }
            }

            return pairs;
        }
    }
}