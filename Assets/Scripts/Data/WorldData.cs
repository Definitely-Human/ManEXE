using System.Collections.Generic;
using ManExe.World;
using UnityEngine;

namespace ManExe.Data
{
    [System.Serializable]
    public class WorldData 
    {
        // === Data ===
        private string worldName = "Default";
        private int levelSeed;

        [System.NonSerialized]
        private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

        // === Properties ===
        public string WorldName { get => worldName; set => worldName = value; }
        public int LevelSeed { get => levelSeed; private set => levelSeed = value; }
        public Dictionary<Vector3Int, Chunk> Chunks { get => chunks;  }



        //===============================
        // === Constructors ===
        //===============================
        public WorldData(string _worldName, int _seed)
        {
            WorldName = _worldName;
            LevelSeed = _seed;
        }

        public WorldData(WorldData _WD)
        {
            WorldName = _WD.WorldName;
            LevelSeed = _WD.LevelSeed;
        }

    }
}
