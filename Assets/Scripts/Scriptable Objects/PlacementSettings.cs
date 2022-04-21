using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    [CreateAssetMenu(fileName = "New PlacementSettings", menuName = "Scriptable/PlacementSettings", order = 0)]
    public class PlacementSettings : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;

        [Header("Prefab Location Settings")]
        [SerializeField] private int _density; // Density per chunk
        [SerializeField] private int _minHeight;
        [SerializeField] private int _maxHeight;

        [Header("Prefab Variation Settings")]
        [SerializeField, Range(0, 1)] float rotateTwardsNormal;
        [SerializeField] Vector2 _rotationRange;
        [SerializeField] Vector3 _minScale;
        [SerializeField] Vector3 _maxScale;

        public string Name { get => _name; set => _name = value; }
        public GameObject Prefab { get => _prefab; set => _prefab = value; }
        public int Density { get => _density; set => _density = value; }
        public int MinHeight { get => _minHeight; set => _minHeight = value; }
        public int MaxHeight { get => _maxHeight; set => _maxHeight = value; }
        public float RotateTwardsNormal { get => rotateTwardsNormal; set => rotateTwardsNormal = value; }
        public Vector2 RotationRange { get => _rotationRange; set => _rotationRange = value; }
        public Vector3 MinScale { get => _minScale; set => _minScale = value; }
        public Vector3 MaxScale { get => _maxScale; set => _maxScale = value; }
    }
}
