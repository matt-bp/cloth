using System.Collections.Generic;
using Cloth.Provider;
using NUnit.Framework;
using UnityEngine;

namespace Cloth.UnitTests.Runtime.Cloth.Provider
{
    public class MassProviderTests
    {
        [Test]
        public void GetMasses_OfTwoTriangles_ReturnsDensityByArea()
        {
            var positions = new Vector3[]
            {
                Vector3.zero,
                Vector3.right,
                Vector3.up
            };
            var triangles = new (int, int, int)[]
            {
                (0, 1, 2)
            };
            var massProvider = new MassProvider(0.5f);

            var result = massProvider.GetMasses(triangles, positions);
            
            Assert.That(result[0], Is.EqualTo(0.083333f).Within(0.00001));
            Assert.That(result[1], Is.EqualTo(0.083333f).Within(0.00001));
            Assert.That(result[2], Is.EqualTo(0.083333f).Within(0.00001));
        }
    }
}