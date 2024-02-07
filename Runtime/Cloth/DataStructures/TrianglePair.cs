using System;
using System.Collections.Generic;

namespace Cloth.DataStructures
{
    public static class TrianglePair
    {
        public static List<(int, int, int, int)> MakeFromSharedEdges(List<(int, int, int)> triangles)
        {
            var quads = new List<(int, int, int, int)>();

            var previousEdges = new Dictionary<(int, int), int>();

            foreach(var triangle in triangles)
            {
                Console.WriteLine("New triangle: " + string.Join(',', triangle));

                var edge0 = (triangle.Item1, triangle.Item2);
                var edge1 = (triangle.Item2, triangle.Item3);
                var edge2 = (triangle.Item3, triangle.Item1);

                Console.WriteLine("\tChecking for: " + edge0);
                if (previousEdges.ContainsKey(edge0)) {
                    Console.WriteLine("\t\tFound it!");
                    var otherIndex = previousEdges[edge0];
                    quads.Add((triangle.Item3, triangle.Item1, triangle.Item2, otherIndex));
                }
                else
                {
                    Console.WriteLine("\t\tAdding to the previous: " + (edge0.Item2, edge0.Item1));
                    previousEdges.Add((edge0.Item2, edge0.Item1), triangle.Item3);
                }

                Console.WriteLine("\tChecking for: " + edge1);
                if (previousEdges.ContainsKey(edge1)) {
                    Console.WriteLine("\t\tFound it!");
                    var otherIndex = previousEdges[edge1];
                    quads.Add((triangle.Item1, triangle.Item2, triangle.Item3, otherIndex));
                }
                else
                {
                    Console.WriteLine("\t\tAdding to the previous: " + (edge1.Item2, edge1.Item1));
                    previousEdges.Add((edge1.Item2, edge1.Item1), triangle.Item1);
                }

                Console.WriteLine("\tChecking for: " + edge2);
                if (previousEdges.ContainsKey(edge2)) {
                    Console.WriteLine("\t\tFound it!");
                    var otherIndex = previousEdges[edge2];
                    quads.Add((triangle.Item2, triangle.Item3, triangle.Item1, otherIndex));
                }
                else
                {
                    Console.WriteLine("\t\tAdding to the previous: " + (edge2.Item2, edge2.Item1));
                    previousEdges.Add((edge2.Item2, edge2.Item1), triangle.Item2);
                }
            }

            return quads;
        }
    }
}