using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    [CreateAssetMenu(fileName = "New LevelSettings", menuName = "LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private int seed;
        [SerializeField] private int worldSize;

        public int Seed { get { return seed; } }
        public int WorldSize { get { return worldSize; } }
    
    }
}
