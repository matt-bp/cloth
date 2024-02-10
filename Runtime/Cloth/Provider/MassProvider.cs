using System.Linq;
using Cloth.DataStructures;
using UnityEngine;

namespace Cloth.Provider
{
    public interface IMassProvider
    {
        float[] GetMasses((int, int, int)[] triangles, Vector3[] positions);
    }

    public class MassProvider : IMassProvider
    {
        private readonly float _surfaceDensity;

        public MassProvider(float surfaceDensity)
        {
            _surfaceDensity = surfaceDensity;
        }

        public float[] GetMasses((int, int, int)[] triangles, Vector3[] positions)
        {
            float GetArea((int, int, int) triangle)
            {
                var v1 = positions[triangle.Item1];
                var v2 = positions[triangle.Item2];
                var v3 = positions[triangle.Item3];
                return Triangle.GetArea(v1, v2, v3);
            }

            var totalArea = triangles.Sum(GetArea);
            var totalMass = _surfaceDensity * totalArea;
            return Enumerable.Range(0, positions.Length).Select(_ => totalMass / positions.Length).ToArray();
        }
    }
}