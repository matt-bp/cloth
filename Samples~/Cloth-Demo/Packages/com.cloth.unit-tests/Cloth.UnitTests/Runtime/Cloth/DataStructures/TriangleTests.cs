
using System.Collections.Generic;
using System.Linq;
using Cloth.DataStructures;
using NUnit.Framework;
using UnityEngine;

namespace Cloth.UnitTests.Runtime.Cloth.DataStructures
{
    public class TriangleTests
    {
        [Test]
        public void GetArea_WithUnitTriangle_ReturnsOneHalf()
        {
            var v0 = Vector3.zero;
            var v1 = Vector3.right;
            var v2 = Vector3.up;

            var area = Triangle.GetArea(v0, v1, v2);
            
            Assert.That(area, Is.EqualTo(0.5f).Within(0.000001));
        }
        
        [Test]
        public void GetTrianglesFromFlatList_WithTwoTriangles_ReturnsThoseGrouped()
        {
            var indices = new List<int>()
            {
                1, 2, 3, 4, 5, 6
            };

            var result = Triangle.GetTrianglesFromFlatList(indices).ToList();
            
            Assert.That(result, Contains.Item((1, 2, 3)));
            Assert.That(result, Contains.Item((4, 5, 6)));
        }
    }
}