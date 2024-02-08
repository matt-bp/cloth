using System;

namespace Cloth.Springs
{
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
            return obj is not SpringPair pair ? base.Equals(obj) : Equals(pair);
        }

        private bool Equals(SpringPair other)
        {
            return FirstIndex == other.FirstIndex && SecondIndex == other.SecondIndex && (other.RestLength - RestLength) < 0.00001;
        }
    }
}