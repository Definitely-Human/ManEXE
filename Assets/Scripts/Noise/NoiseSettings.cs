using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    [CreateAssetMenu(fileName = "New NoiseSettings", menuName = "Scriptable/NoiseSettings", order = 0)]
    public class NoiseSettings : ScriptableObject
    {
        [SerializeField] private float _scale;
        [SerializeField] private int _octaves;
        [SerializeField, Range(0,1)] private float _persistance;
        [SerializeField] private float _lacunarity;
        [SerializeField] private Vector2 _offset;

        public float Scale { get => _scale; set => _scale = value; }
        public int Octaves { get => _octaves; set => _octaves = value; }
        public float Persistance { get => _persistance; set => _persistance = value; }
        public float Lacunarity { get => _lacunarity; set => _lacunarity = value; }
        public Vector2 Offset { get => _offset; set => _offset = value; }

        private void OnValidate()
        {
            if(Lacunarity < 1)
            {
                Lacunarity = 1;
            }
            if (Octaves < 1)
            {
                Octaves = 1;
            }
            if (Persistance < 0)
            {
                Persistance = 0;
            }
            if (Persistance > 1)
            {
                Persistance = 1;
            }
        }
    }
}
