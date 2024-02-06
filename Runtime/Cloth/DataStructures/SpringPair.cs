using System;
using UnityEngine;

namespace Cloth.DataStructures
{
    [Serializable]
    public class SpringPair
    {
        [SerializeField] public int firstIndex;
        [SerializeField] public int secondIndex;
    }
}