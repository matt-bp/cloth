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

        [Test]
        public void CreateBendSprings_TwoQuads_HasTwoBendSprings()
        {
            var positions = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 3, 0),
                new Vector3(5, 0, 0),
                new Vector3(5, 3, 0),
                new Vector3(10, 0, 0),
                new Vector3(10, 3, 0),
            };
            var triangles = new[]
            {
                (0, 1, 2),
                (2, 3, 1),
                (2, 3, 4),
                (4, 5, 3),
            };
            var provider = new SpringProvider();

            var pairs = provider.CreateBendSprings(triangles, positions);

            pairs[0].Should().BeEquivalentTo(new SpringPair { FirstIndex = 0, SecondIndex = 4, RestLength = 10});
            pairs[1].Should().BeEquivalentTo(new SpringPair { FirstIndex = 1, SecondIndex = 5, RestLength = 10});
        }
    }
}