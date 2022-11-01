using ManExe.Data;
using ManExe.Noise;
using UnityEngine;

namespace ManExe.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New LevelSettings", menuName = "Scriptable/LevelSettings", order = 0)]
    public class LevelSettings : UpdatableData
    {
        [Space]
        [SerializeField] private int _seed;
        [SerializeField] private int _worldSizeInChunksX;
        [SerializeField] private int _worldSizeInChunksY;
        [SerializeField] private NoiseSettings _noiseSettings;

        [SerializeField] private int _baseTerrainHeight;
        [SerializeField] private int _terrainHeightRange;
        [Space]
        [SerializeField] private PlacableConfigData[] _placableConfigData;

        public int Seed { get { return _seed; } set { _seed = value; } }
        public int WorldSizeInChunksX { get { return _worldSizeInChunksX; } set { _worldSizeInChunksX = value; } }
        public int WorldSizeInChunksY { get { return _worldSizeInChunksY; } set { _worldSizeInChunksY = value; } }
        public NoiseSettings NoiseSettings { get => _noiseSettings; set => _noiseSettings = value; }
        public int BaseTerrainHeight { get => _baseTerrainHeight; set => _baseTerrainHeight = value; }
        public int TerrainHeightRange { get => _terrainHeightRange; set => _terrainHeightRange = value; }
        public PlacableConfigData[] PlacableConfigData { get => _placableConfigData; }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            

            base.OnValidate();
        }
    }
#endif
}
