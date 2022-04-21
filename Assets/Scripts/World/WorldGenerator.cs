using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ManExe
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private World world;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private bool _destroyAfterGeneration;
        [SerializeField] private bool _generatePlacements;

        private PlacementGenerator _placementGenerator;

        private float[,] heightMap;

        private void Awake()
        {

            _placementGenerator = GetComponent<PlacementGenerator>();
        }

        private void Start()
        {
            Generate();
            if(_generatePlacements)
                _placementGenerator.Generate();


            if (_destroyAfterGeneration)
                Destroy(gameObject);
        }

        public void Generate() // Generates world and destroys this game object
        {

            loadingScreen.SetActive(true);

            heightMap = Noise.GenerateNoiseMap(
                world.WorldSizeInVoxelsX + 1,
                world.WorldSizeInVoxelsY + 1,
                world.Settings.Seed, 
                world.Settings.NoiseSettings.Scale,
                world.Settings.NoiseSettings.Octaves,
                world.Settings.NoiseSettings.Persistance,
                world.Settings.NoiseSettings.Lacunarity,
                world.Settings.NoiseSettings.Offset);

            for(int x = 0; x < world.Settings.WorldSizeInChunksX; x++)
            {
                for (int z = 0; z < world.Settings.WorldSizeInChunksY; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                    world.AddChunk(chunkPos,heightMap);
                }
            }
            
            Debug.Log(string.Format("{0} x {1} world generated.", world.WorldSizeInVoxelsX, world.WorldSizeInVoxelsY));

            world.SpawnPosition = new Vector3(GameData.ChunkWidth * world.Settings.WorldSizeInChunksX /2, 
                heightMap[GameData.ChunkWidth * world.Settings.WorldSizeInChunksX / 2, GameData.ChunkWidth * world.Settings.WorldSizeInChunksY / 2] + 1, 
                GameData.ChunkWidth * world.Settings.WorldSizeInChunksY / 2);
            Instantiate(playerPrefab, world.SpawnPosition, Quaternion.identity);

            loadingScreen.SetActive(false);
        }

        

    }

   
}
