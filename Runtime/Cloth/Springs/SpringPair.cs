using System;
using UnityEngine;

namespace Cloth.Springs
{
    // [Serializable]
    public class SpringPair
    {
        public int FirstIndex;
        public int SecondIndex;
        public float RestLength;

        public override string ToString()
        {
            return $"({FirstIndex} to {SecondIndex}), at length {RestLength}";
        }

        public override bool Equals(object obj)
        {
            bool IsEqual(SpringPair other)
            {
                return other.FirstIndex == FirstIndex && other.SecondIndex == SecondIndex &&
                       (other.RestLength - RestLength) < 0.00001;
            }

            return obj is not SpringPair pair ? base.Equals(obj) : IsEqual(pair);
        }
    }
}