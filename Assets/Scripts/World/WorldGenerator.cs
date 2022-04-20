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

        private PlacementGenerator _placementGenerator;

        private float[,] heightMap;

        private void Awake()
        {

            heightMap = new float[GameData.ChunkHeight * world.Settings.WorldSize, GameData.ChunkHeight * world.Settings.WorldSize];
            _placementGenerator = GetComponent<PlacementGenerator>();
        }

        private void Start()
        {
            Generate();
            _placementGenerator.Generate();


            if (_destroyAfterGeneration)
                Destroy(gameObject);
        }

        private void Generate() // Generates world and destroys this game object
        {
            loadingScreen.SetActive(true);
            heightMap = Noise.GenerateNoiseMap(
                GameData.ChunkHeight * world.Settings.WorldSize,
                GameData.ChunkHeight * world.Settings.WorldSize,
                world.Settings.Seed, 75, 4, 0.5f, 1.75f, new Vector2());

            
            for(int x = 0; x < world.Settings.WorldSize; x++)
            {
                for (int z = 0; z < world.Settings.WorldSize; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                    world.AddChunk(chunkPos,heightMap);
                }
            }
            loadingScreen.SetActive(false);
            Debug.Log(string.Format("{0} x {0} world generated.", (world.Settings.WorldSize * GameData.ChunkWidth)));

            world.SpawnPosition = new Vector3(GameData.ChunkWidth * world.Settings.WorldSize /2, 
                heightMap[GameData.ChunkWidth * world.Settings.WorldSize / 2, GameData.ChunkWidth * world.Settings.WorldSize / 2] + 1, 
                GameData.ChunkWidth * world.Settings.WorldSize / 2);
            Instantiate(playerPrefab, world.SpawnPosition, Quaternion.identity);

            
        }

        

    }

   
}
