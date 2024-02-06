using System.Collections.Generic;
using System.Linq;

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
    }
}