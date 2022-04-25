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
        private GrassGenerator _grassGenerator;

        private float[,] heightMap;

        private void Awake()
        {

            _placementGenerator = GetComponent<PlacementGenerator>();
            _grassGenerator = GetComponent<GrassGenerator>();
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

            heightMap = Noise.GenerateNoiseMap(world.Settings);

            for(int x = 0; x < world.Settings.WorldSizeInChunksX; x++)
            {
                for (int z = 0; z < world.Settings.WorldSizeInChunksY; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                    Chunk chunk = world.AddChunk(chunkPos,heightMap);
                    _grassGenerator.GenerateGrass(chunk.GameObject);
                }
            }
            
            Debug.Log(string.Format("{0} x {1} world generated.", world.WorldSizeInVoxelsX, world.WorldSizeInVoxelsY));

            world.SpawnPosition = new Vector3(GameData.ChunkWidth * world.Settings.WorldSizeInChunksX /2, 
                heightMap[GameData.ChunkWidth * world.Settings.WorldSizeInChunksX / 2, GameData.ChunkWidth * world.Settings.WorldSizeInChunksY / 2] + 10, 
                GameData.ChunkWidth * world.Settings.WorldSizeInChunksY / 2);
            Instantiate(playerPrefab, world.SpawnPosition, Quaternion.identity);

            loadingScreen.SetActive(false);
        }

        

    }

   
}
