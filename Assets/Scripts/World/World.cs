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
        // === Properties ===
        public LevelSettings Settings { get => settings; set => settings = value; }
        public Vector3 SpawnPosition { get { return _spawnPosition; } set { _spawnPosition = value; } }
        //===============================
        // === GameObject Methods ===
        //===============================

        private void Awake()
        {
            worldData = new WorldData("Default", 123);
        }


        // === Public Methods ===
        //===============================
        public void AddChunk(Vector3Int chunkPos, float[,] heightMap) {
            Chunk chunk = new Chunk(chunkPos, heightMap, GetComponent<World>());
            worldData.Chunks.Add(chunkPos,chunk);
            chunk.SetChunkParent(gameObject);

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
