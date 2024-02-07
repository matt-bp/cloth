using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cloth.DataStructures
{
    public static class Triangle
    {
        /// <summary>
        /// Returns the grouping of triangles from the flat list of indices.
        /// </summary>
        /// <param name="indicesList"></param>
        /// <returns></returns>
        public static IEnumerable<(int, int ,int)> GetTrianglesFromFlatList(List<int> indicesList)
        {
            for (var i = 0; i < indicesList.Count; i += 3)
            {
                var asList = indicesList.Skip(i).Take(3).ToList();
                yield return (asList[0], asList[1], asList[2]);
            }
        }
        
        public static float GetArea(Vector3 pos0, Vector3 pos1, Vector3 pos2)
        {
            var a = pos0 - pos1;
            var b = pos1 - pos2;
            var c = pos2 - pos0;

            return Herons(a.magnitude, b.magnitude, c.magnitude);
        }
        
        private static float Herons(float a, float b, float c)
        {
            var p = 0.5f * (a + b + c);
            return MathF.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
}