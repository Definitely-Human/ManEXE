using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.Noise
{
    [CreateAssetMenu(fileName = "New NoiseSettings", menuName = "Scriptable/NoiseSettings", order = 0)]
    public class NoiseSettings : UpdatableData
    {
        [SerializeField] private float _scale;
        [SerializeField, Range(0, 12)] private int _octaves;
        [SerializeField, Range(0,1)] private float _persistance;
        [SerializeField] private float _lacunarity;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private AnimationCurve _heightCurve;

        public float Scale { get => _scale; set => _scale = value; }
        public int Octaves { get => _octaves; set => _octaves = value; }
        public float Persistance { get => _persistance; set => _persistance = value; }
        public float Lacunarity { get => _lacunarity; set => _lacunarity = value; }
        public Vector2 Offset { get => _offset; set => _offset = value; }
        public AnimationCurve HeightCurve { get => _heightCurve; set => _heightCurve = value; }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (_scale <= 0.0001f)
            {
                _scale = 0.0001f;
            }
            if (Lacunarity < 1)
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

            base.OnValidate();
        }
#endif
    }
}
