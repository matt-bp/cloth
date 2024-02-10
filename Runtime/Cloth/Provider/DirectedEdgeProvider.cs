using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Provider
{
    public static class DirectedEdgeProvider
    {
        public static (DirectedEdge, DirectedEdge) GetEdgesFromTriangle((int, int, int) triangle, Vector3 v1, Vector3 v2, Vector3 v3)
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
                return (new DirectedEdge { Start = triangle.Item2, End = triangle.Item3, Edge = edge2 },
                    new DirectedEdge { Start = triangle.Item3, End = triangle.Item1, Edge = edge3 });
            }

            if (edge2Mag > edge1Mag && edge2Mag > edge3Mag)
            {
                // v1 is in the middle
                return (new DirectedEdge { Start = triangle.Item3, End = triangle.Item1, Edge = edge3 },
                    new DirectedEdge { Start = triangle.Item1, End = triangle.Item2, Edge = edge1 });
            }

            return (new DirectedEdge { Start = triangle.Item1, End = triangle.Item2, Edge = edge1 },
                new DirectedEdge { Start = triangle.Item2, End = triangle.Item3, Edge = edge2 });
        }
    }
}