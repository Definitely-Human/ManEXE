using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    [System.Serializable]
    public class ChunkData
    {
        // === Data ===
        private int x;
        private int y;
        private int z;
        
        [HideInInspector]
        private TerrainPoint[,,] voxelMap = new TerrainPoint[GameData.ChunkWidth + 1, GameData.ChunkHeight + 1, GameData.ChunkWidth + 1];

        // === Properties ===

        public Vector3Int GlobalChunkPos
        {
            get { return new Vector3Int(x, y, z); }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public TerrainPoint[,,] VoxelMap { get => voxelMap; }

        public ChunkData(Vector3Int pos)
        {
            GlobalChunkPos = pos;
        }
        public ChunkData(int _x, int _y, int _z) { x = _x; y = _y; z = _z; }

    }

}
