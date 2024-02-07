using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.Provider;
using Cloth.Springs;
using Cloth.Technique;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Cloth.UnitTests.Runtime.Cloth.Technique
{
    public class MassSpringTests
    {
        /// <summary>
        /// Test case for Newton's First Law of Motion.
        /// </summary>
        [Test]
        public void Step_NoExternalForcesAndZeroDt_ReturnsSamePositions()
        {
            var vertices = new[]
            {
                Vector3.up,
                Vector3.down,
                Vector3.forward
            };
            var triangles = new[]
            {
                0, 1, 2
            };
            
            var stubMassProvider = Substitute.For<IMassProvider>();
            stubMassProvider.GetMasses(AnyTriangleArray(), AnyVectorArray())
                .Returns(new float[] { 0.5f, 0.5f, 0.5f });
            var stubSpringProvider = Substitute.For<ISpringProvider>();
            stubSpringProvider.CreateStretchSprings(AnyTriangleArray(), AnyVectorArray()).Returns(
                new List<SpringPair>()
                {
                    new() { FirstIndex = 0, SecondIndex = 1, RestLength = 1 }
                });
            stubSpringProvider.CreateShearSprings(AnyTriangleArray(), AnyVectorArray())
                .Returns(Enumerable.Empty<SpringPair>().ToList());
            var massSpring = new MassSpring(stubMassProvider, stubSpringProvider, triangles, vertices, 4, 1);

            massSpring.Step(0, Array.Empty<Vector3>());

            Assert.That(massSpring.Positions, Is.EquivalentTo(vertices));
        }

        #region Helpers

        private static (int, int, int)[] AnyTriangleArray() => Arg.Any<(int, int, int)[]>();
        private static Vector3[] AnyVectorArray() => Arg.Any<Vector3[]>();

        #endregion
    }
}