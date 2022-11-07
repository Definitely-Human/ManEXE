using ManExe.Data;
using ManExe.Noise;
using UnityEngine;

namespace ManExe.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New LevelSettings", menuName = "Scriptable/LevelSettings", order = 0)]
    public class LevelSettings : UpdatableData // WARNING! Do not edit variable names or data in Scriptable Objects will be lost
    {
        [Space]
        [SerializeField] private int seed;
        [SerializeField] private int worldSizeInChunksX;
        [SerializeField] private int worldSizeInChunksZ;
        [SerializeField] private NoiseSettings noiseSettings;

        [SerializeField] private int baseTerrainHeight;
        [SerializeField] private int terrainHeightRange;
        [Space]
        [SerializeField] private PlacableConfigData[] placableConfigData;

        public int Seed { get => seed;
            set => seed = value;
        }
        public int WorldSizeInChunksX { 
            get => worldSizeInChunksX;
            set => worldSizeInChunksX = value;
        }
        public int WorldSizeInChunksZ { 
            get => worldSizeInChunksZ;
            set => worldSizeInChunksZ = value;
        }
        public NoiseSettings NoiseSettings { get => noiseSettings; set => noiseSettings = value; }
        public int BaseTerrainHeight { get => baseTerrainHeight; set => baseTerrainHeight = value; }
        public int TerrainHeightRange { get => terrainHeightRange; set => terrainHeightRange = value; }
        public PlacableConfigData[] PlacableConfigData => placableConfigData;
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            

            base.OnValidate();
        }
    
#endif
    }
}
