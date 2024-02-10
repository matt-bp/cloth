using Cloth.DataStructures;
using Cloth.Provider;
using NUnit.Framework;
using UnityEngine;
using FluentAssertions;

namespace Cloth.UnitTests.Runtime.Cloth.Provider
{
    public class DirectedEdgeProviderTests
    {
        [Test]
        public void GetEdgesFromTriangle_With90OnPoint3_ReturnsEdgesConnectedToPoint3()
        {
            var triangle = (1, 2, 3);
            var v1 = Vector3.left;
            var v2 = Vector3.up;
            var v3 = Vector3.zero;

            var edges = DirectedEdgeProvider.GetEdgesFromTriangle(triangle, v1, v2, v3);

            edges[0].Should().BeEquivalentTo(new DirectedEdge { Start = 2, End = 3, Edge = Vector3.down });
            edges[1].Should().BeEquivalentTo(new DirectedEdge { Start = 3, End = 1, Edge = Vector3.left });
        }

        [Test]
        public void GetEdgesFromTriangle_With90OnPoint1_ReturnsEdgesConnectedToPoint1()
        {
            var triangle = (1, 2, 3);
            var v1 = Vector3.zero;
            var v2 = Vector3.left;
            var v3 = Vector3.up;
            
            var edges = DirectedEdgeProvider.GetEdgesFromTriangle(triangle, v1, v2, v3);

            edges[0].Should().BeEquivalentTo(new DirectedEdge { Start = 3, End = 1, Edge = Vector3.down });
            edges[1].Should().BeEquivalentTo(new DirectedEdge { Start = 1, End = 2, Edge = Vector3.left });
        }

        [Test]
        public void GetEdgesFromTriangle_With90OnPoint2_ReturnsEdgesConnectedToPoint2()
        {
            var triangle = (1, 2, 3);
            var v1 = Vector3.up;
            var v2 = Vector3.zero;
            var v3 = Vector3.left;
            
            var edges = DirectedEdgeProvider.GetEdgesFromTriangle(triangle, v1, v2, v3);
            
            edges[0].Should().BeEquivalentTo(new DirectedEdge { Start = 1, End = 2, Edge = Vector3.down });
            edges[1].Should().BeEquivalentTo(new DirectedEdge { Start = 2, End = 3, Edge = Vector3.left });
        }
    }
}