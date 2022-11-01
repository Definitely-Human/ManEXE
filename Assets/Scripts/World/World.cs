using ManExe.Core;
using ManExe.Data;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.World
{
    public class World : MonoBehaviour
    {
        private const int TextureArrayWidth = 4096;

        // === References ===
        [SerializeField] private LevelSettings settings;

        // === Data ===
        private WorldData worldData;
        private Vector3 _spawnPosition;
        private GameObject _placementsContainer;

        // === Textures ===
        [SerializeField]
        private Texture2D[] _terrainTextures;
        private Texture2DArray _terrainTexArray;

        [SerializeField]
        private Texture2D[] _terrainNormals;
        private Texture2DArray _terrainNorArray;

        [SerializeField]
        private float[] _terrainScales;

        [SerializeField]
        private TerrainType[] _terrainTypes;

        // === Properties ===
        public LevelSettings Settings { get => settings; set => settings = value; }
        public Vector3 SpawnPosition { get { return _spawnPosition; } set { _spawnPosition = value; } }

        public int WorldSizeInVoxelsX { get { return Settings.WorldSizeInChunksX * GameData.ChunkWidth; } }
        public int WorldSizeInVoxelsY { get { return Settings.WorldSizeInChunksX * GameData.ChunkWidth; } }

        public GameObject Placements {
            get { 
                if (_placementsContainer != null) { return _placementsContainer; }
                else
                {
                    _placementsContainer = new GameObject();
                    _placementsContainer.transform.SetParent(transform);
                    _placementsContainer.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                    _placementsContainer.transform.name = "Placements";
                    return _placementsContainer;
                }
            }
        }

        public Texture2D[] TerrainTextures { get => _terrainTextures;  }
        public Texture2DArray TerrainTexArray { get => _terrainTexArray;  }
        public TerrainType[] TerrainTypes { get => _terrainTypes; set => _terrainTypes = value; }
        public Texture2D[] TerrainNormals { get => _terrainNormals; set => _terrainNormals = value; }
        public Texture2DArray TerrainNorArray { get => _terrainNorArray; set => _terrainNorArray = value; }
        public float[] TerrainScales { get => _terrainScales; set => _terrainScales = value; }

        //===============================
        // === GameObject Methods ===
        //===============================

        private void Awake()
        {
            if(_terrainTextures.Length > 0) // Error when trying to create Texture2DArray with 0 lenght
                PopulateTextureArray();
            if (_terrainTextures.Length > 0)
                PopulateNormalArray(); 
             worldData = new WorldData("Default", Settings.Seed);
        }


        // === Public Methods ===
        //===============================
        public Chunk AddChunk(Vector3Int chunkPos, float[,] heightMap) {
            Chunk chunk = new Chunk(chunkPos, heightMap, GetComponent<World>());
            worldData.Chunks.Add(chunkPos,chunk);
            chunk.SetChunkParent(gameObject);
            return chunk;

        }

        public Chunk GetChunkFromVector3(Vector3 _pos)
        {
            int x = (int)_pos.x;
            int y = (int)_pos.y;
            int z = (int)_pos.z;

            return worldData.Chunks[new Vector3Int(x, y, z)];
        }

        // === Private Methods ===
        //===============================

        private void PopulateTextureArray()
        {
            _terrainTexArray = new Texture2DArray(TextureArrayWidth, TextureArrayWidth, _terrainTextures.Length, TextureFormat.ARGB32, false);
            for(int i =0; i < _terrainTextures.Length; i++)
            {
                _terrainTexArray.SetPixels(_terrainTextures[i].GetPixels(0), i, 0);
            }
            _terrainTexArray.Apply();
        }

        private void PopulateNormalArray()
        {
            _terrainNorArray = new Texture2DArray(TextureArrayWidth, TextureArrayWidth, _terrainNormals.Length, TextureFormat.ARGB32, false);
            for (int i = 0; i < _terrainNormals.Length; i++)
            {
                _terrainNorArray.SetPixels(_terrainNormals[i].GetPixels(0), i, 0);
            }
            _terrainNorArray.Apply();
        }
    }
}
