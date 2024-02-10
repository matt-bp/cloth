using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Provider
{
    public static class DirectedEdgeProvider
    {
        public static List<DirectedEdge> GetEdgesFromTriangle((int, int, int) triangle, Vector3 v1, Vector3 v2,
            Vector3 v3)
        {
            // Indices will be returned according to the edge orientation here
            var edge1 = v2 - v1;
            var edge2 = v3 - v2;
            var edge3 = v1 - v3;

            var edge1Mag = edge1.magnitude;
            var edge2Mag = edge2.magnitude;
            var edge3Mag = edge3.magnitude;

            if (edge1Mag > edge2Mag && edge1Mag > edge3Mag)
            {
                // v3 is in the middle
                return new List<DirectedEdge>
                {
                    new() { Start = triangle.Item2, End = triangle.Item3, Edge = edge2 },
                    new() { Start = triangle.Item3, End = triangle.Item1, Edge = edge3 }
                };
            }

            if (edge2Mag > edge1Mag && edge2Mag > edge3Mag)
            {
                // v1 is in the middle
                return new List<DirectedEdge>
                {
                    new() { Start = triangle.Item3, End = triangle.Item1, Edge = edge3 },
                    new() { Start = triangle.Item1, End = triangle.Item2, Edge = edge1 }
                };
            }

            // v2 is in the middle
            return new List<DirectedEdge>
            {
                new() { Start = triangle.Item1, End = triangle.Item2, Edge = edge1 },
                new() { Start = triangle.Item2, End = triangle.Item3, Edge = edge2 }
            };
        }

        public static List<DirectedEdge> DuplicateAndReverse(this List<DirectedEdge> toBeDuplicated)
        {
            var all = toBeDuplicated;
            
            var reversed = toBeDuplicated.Select(t => new DirectedEdge
            {
                Start = t.End,
                End = t.Start,
                Edge = -t.Edge
            }).ToList();

            all.AddRange(reversed);

            return all;
        }
    }
}