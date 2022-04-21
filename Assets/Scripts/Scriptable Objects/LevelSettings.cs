using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManExe
{
    [CreateAssetMenu(fileName = "New LevelSettings", menuName = "Scriptable/LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private int _seed;
        [SerializeField] private int _worldSizeInChunksX;
        [SerializeField] private int _worldSizeInChunksY;
        [SerializeField] private NoiseSettings _noiseSettings;

        
        public int Seed { get { return _seed; } set { _seed = value; } }
        public int WorldSizeInChunksX { get { return _worldSizeInChunksX; } set { _worldSizeInChunksX = value; } }
        public int WorldSizeInChunksY { get { return _worldSizeInChunksY; } set { _worldSizeInChunksY = value; } }
        public NoiseSettings NoiseSettings { get => _noiseSettings; set => _noiseSettings = value; }
    }
}
