using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject loadingScreen;
        public World world;

        public float[,] heightMap;

        
        private void Start()
        {
            heightMap = new float[GameData.ChunkHeight * world.Settings.WorldSize, GameData.ChunkHeight * world.Settings.WorldSize];
            Generate();
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

            Destroy(gameObject);
        }

        

    }

   
}
