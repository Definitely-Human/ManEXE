using ManExe.Core;
using ManExe.Entity.Inventory;
using ManExe.Shader.StylizedBladesGrass;
using ManExe.UI.Inventory;
using UnityEngine;

namespace ManExe.World
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private World world;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private bool _destroyAfterGeneration;
        [SerializeField] private bool _generatePlacements;
        [SerializeField] private bool _generateGrass;

        private PlacementGenerator _placementGenerator;
        private GrassGenerator _grassGenerator;

        private float[,] heightMap;
        private GameObject _player;

        private void Awake()
        {
            CreatePlayer();
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

            heightMap = Noise.Noise.GenerateNoiseMap(world.Settings);

            for(int x = 0; x < world.Settings.WorldSizeInChunksX; x++)
            {
                for (int z = 0; z < world.Settings.WorldSizeInChunksZ; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                    Chunk chunk = world.AddChunk(chunkPos,heightMap);
                    if( _generateGrass)
                        _grassGenerator.GenerateGrass(chunk.GameObject);
                }
            }
            
            Debug.Log(string.Format("{0} x {1} world generated.", world.WorldSizeInVoxelsX, world.WorldSizeInVoxelsZ));

            world.SpawnPosition = new Vector3((int)(GameData.ChunkWidth * world.Settings.WorldSizeInChunksX /2), 
                heightMap[GameData.ChunkWidth * world.Settings.WorldSizeInChunksX / 2, GameData.ChunkWidth * world.Settings.WorldSizeInChunksZ / 2] + 10, 
                (int)(GameData.ChunkWidth * world.Settings.WorldSizeInChunksZ / 2));
            _player.GetComponent<CharacterController>().Move( new Vector3(world.SpawnPosition.x,world.SpawnPosition.y,world.SpawnPosition.z));
            loadingScreen.SetActive(false);
        }

        private void CreatePlayer()
        {
            _player = Instantiate(playerPrefab);
            _player.tag = "Player";
        }

    }

   
}
