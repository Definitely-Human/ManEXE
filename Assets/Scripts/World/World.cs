using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ManExe
{
    public class World : MonoBehaviour
    {
        // === References ===
        [SerializeField] private LevelSettings settings;

        // === Data ===
        private WorldData worldData;
        private Vector3 _spawnPosition;
        private GameObject _placementsContainer;
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

        //===============================
        // === GameObject Methods ===
        //===============================

        private void Awake()
        {
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
    }
}
