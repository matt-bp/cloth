using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.Technique;
using NUnit.Framework;
using UnityEngine;

namespace Cloth.UnitTests.Runtime.Cloth.Technique
{
    public class MassSpringTests
    {
        [Test, Ignore("Working on other tests")]
        public void Step_NoExternalForces_ReturnsSamePositions()
        {
            var positions = new List<Vector3>
            {
                Vector3.up,
                Vector3.down,
                Vector3.forward
            };
            var triangles = new[]
            {
                0, 1, 2
            };
            var cloth = new MassSpring(triangles, positions.ToArray(), 4f, 1f, 0.5f);
            var dt = 0.01f;
            
            cloth.Step(dt, Array.Empty<Vector3>());
            
            Assert.That(cloth.Positions, Is.EquivalentTo(positions.ToArray()));
        }
    }
}