using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class WorldGenerator : MonoBehaviour
    {
        

        Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
        public GameObject loadingScreen;
        public GameObject world;
        void Start()
        {
            Generate();
        }

        void Generate()
        {
            world.GetComponent<World>().heightMap = Noise.GenerateNoiseMap(
                GameData.ChunkHeight * GameData.WorldSizeInChunks,
                GameData.ChunkHeight * GameData.WorldSizeInChunks,
                GameData.seed, 75, 4, 0.5f, 1.75f, new Vector2());

            loadingScreen.SetActive(true);
            for(int x = 0; x <GameData.WorldSizeInChunks; x++)
            {
                for (int z = 0; z < GameData.WorldSizeInChunks; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                    chunks.Add(chunkPos, new Chunk(chunkPos, world.GetComponent<World>()));
                    chunks[chunkPos].chunkObject.transform.SetParent(world.transform);
                }
            }
            loadingScreen.SetActive(false);
            Debug.Log(string.Format("{0} x {0} world generated.", (GameData.WorldSizeInChunks * GameData.ChunkWidth)));
        }

        public Chunk GetChunkFromVector3(Vector3 _pos)
        {
            int x = (int)_pos.x;
            int y = (int)_pos.y;
            int z = (int)_pos.z;

            return chunks[new Vector3Int(x, y, z)];
        }

    }

   
}
