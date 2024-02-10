using System.Collections.Generic;
using UnityEngine;

namespace Cloth.DataStructures
{
    public class DirectedEdge
    {
        public DirectedEdge()
        {
        }

        public DirectedEdge(int start, int end, Vector3 edge)
        {
            Start = start;
            End = end;
            Edge = edge;
        }

        public int Start { get; set; }
        public int End { get; set; }
        public Vector3 Edge { get; set; }
    }
}