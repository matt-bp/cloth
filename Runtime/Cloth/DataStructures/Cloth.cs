using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cloth.DataStructures
{
    public static class Cloth
    {
        public static float[] GetMasses(List<(int, int, int)> triangles, Vector3[] positions, float surfaceDensity)
        {
            float GetArea((int, int, int) triangle)
            {
                var v1 = positions[triangle.Item1];
                var v2 = positions[triangle.Item2];
                var v3 = positions[triangle.Item3];
                return Triangle.GetArea(v1, v2, v3);
            }

            var totalArea = triangles.Sum(GetArea);
            var totalMass = surfaceDensity * totalArea;
            return Enumerable.Range(0, positions.Length).Select(_ => totalMass / positions.Length).ToArray();
        }
    }
}