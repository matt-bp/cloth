using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Provider
{
    public class DirectedEdgeProvider
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
            
            // if (edge1Mag > edge2Mag && edge1Mag > edge3Mag)
            {
                // v3 is in the middle
                return (
                    new DirectedEdge { Start = triangle.Item2, End = triangle.Item3, Edge = edge2 },
                    new DirectedEdge { Start = triangle.Item3, End = triangle.Item1, Edge = edge3 });
            }
            // else if (edge2 > edge1 && edge2 > edge3)
            // {
            //     pairs.Add(new SpringPair
            //         { FirstIndex = triangle.Item3, SecondIndex = triangle.Item1, RestLength = edge3 });
            //     pairs.Add(new SpringPair
            //         { FirstIndex = triangle.Item1, SecondIndex = triangle.Item2, RestLength = edge1 });
            // }
            // else
            // {
            //     pairs.Add(new SpringPair
            //         { FirstIndex = triangle.Item1, SecondIndex = triangle.Item2, RestLength = edge1 });
            //     pairs.Add(new SpringPair
            //         { FirstIndex = triangle.Item2, SecondIndex = triangle.Item3, RestLength = edge2 });
            // }
        }
    }
}