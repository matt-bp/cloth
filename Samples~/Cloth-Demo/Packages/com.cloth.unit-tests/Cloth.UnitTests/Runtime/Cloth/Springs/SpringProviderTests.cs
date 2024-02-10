using Cloth.Springs;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;

namespace Cloth.UnitTests.Runtime.Cloth.Springs
{
    public class SpringProviderTests
    {
        [Test]
        public void CreateStretchSprings_WithSimpleTriangle_ReturnsSpringsOnShortestSides()
        {
            var positions = new[]
            {
                Vector3.zero,
                Vector3.right,
                Vector3.up
            };
            var triangles = new[]
            {
                (0, 1, 2)
            };
            var provider = new SpringProvider();

            var result = provider.CreateStretchSprings(triangles, positions);

            result[0].Should().BeEquivalentTo(new SpringPair { FirstIndex = 2, SecondIndex = 0, RestLength = 1.0f });
            result[1].Should().BeEquivalentTo(new SpringPair { FirstIndex = 0, SecondIndex = 1, RestLength = 1.0f });
        }
    }
}