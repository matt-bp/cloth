using System;
using UnityEngine;

namespace Cloth.Springs
{
    [Serializable]
    public class SpringPair
    {
        [SerializeField] public int firstIndex;
        [SerializeField] public int secondIndex;
    }
}