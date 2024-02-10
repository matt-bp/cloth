using System.Collections.Generic;
using UnityEngine;

namespace Cloth.DataStructures
{
    public class DirectedEdge
    {
        public int Start { get; set; }
        public int End { get; set; }
        public Vector3 Edge { get; set; }
    }
}