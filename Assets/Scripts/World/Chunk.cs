using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class Chunk
    {
        // === Game Components ===
        private GameObject _chunkObject;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;
        private MeshRenderer meshRenderer;

        // === References ===
        private World world;


        // === Data ===
        private ChunkData chunkData;

        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();
        private List<Vector2> uvs = new List<Vector2>();

        // === Properties ===
        public Vector3Int Position { get { return chunkData.GlobalChunkPos; } }
        private int Width { get { return GameData.ChunkWidth; } }
        private int Height { get { return GameData.ChunkHeight; } }
        private float TerrainSurface { get { return GameData.TerrainSurface; } }

        public GameObject GameObject { get => _chunkObject; set => _chunkObject = value; }

        //===============================
        // === Constructors ===
        //===============================
        public Chunk(Vector3Int _pos, float[,] heightMap, World _world)
        {
            world = _world;

            _chunkObject = new GameObject();
            chunkData = new ChunkData(_pos);
            _chunkObject.transform.position = _pos;
            _chunkObject.name = string.Format("Chunk {0}, {1}", _pos.x, _pos.z);
            _chunkObject.layer = 6;

            meshFilter = _chunkObject.AddComponent<MeshFilter>();
            meshCollider = _chunkObject.AddComponent<MeshCollider>();
            meshRenderer = _chunkObject.AddComponent<MeshRenderer>();
            meshRenderer.material = Resources.Load<Material>("Materials/TerrainMaterial");
            
            meshRenderer.material.SetTexture("_Albedo", world.TerrainTexArray);
            meshRenderer.material.SetTexture("_Normal", world.TerrainNorArray);
            //meshRenderer.material.SetFloatArray("_Tiling", world.TerrainScales);
            _chunkObject.transform.tag = "Terrain";
            PopulateTerrainMap(heightMap);
            CreateMeshData();

        }
        // ======= Public Methods ======
        // =============================
        public void PlaceTerrain(Vector3 pos)
        {

            Vector3Int v3Int = new Vector3Int(Mathf.CeilToInt(pos.x), Mathf.CeilToInt(pos.y), Mathf.CeilToInt(pos.z));
            v3Int -= Position;
            chunkData.VoxelMap[v3Int.x, v3Int.y, v3Int.z].DistToSurface = 0f;
            CreateMeshData();

        }

        public void RemoveTerrain(Vector3 pos)
        {

            Vector3Int v3Int = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
            v3Int -= Position;
            chunkData.VoxelMap[v3Int.x, v3Int.y, v3Int.z].DistToSurface = 1f;
            CreateMeshData();

        }

        public void PopulateTerrainMap(float[,] heightMap)
        {

            // The data points for terrain are stored at the corners of our "cubes", so the terrainMap needs to be 1 larger
            // than the width/height of our mesh.
            for (int x = 0; x < Width + 1; x++)
            {
                for (int z = 0; z < Width + 1; z++)
                {
                    for (int y = 0; y < Height + 1; y++)
                    {
                        float thisHeight;

                        // Get a terrain height using from height map from the world generator.
                        thisHeight = heightMap[x + Position.x, z + Position.z];

                        // Set the value of this point in the terrainMap.
                        int blockTypeInd = Mathf.Clamp(Mathf.RoundToInt((float)(y - world.Settings.BaseTerrainHeight) / (float)world.Settings.TerrainHeightRange * world.TerrainTextures.Length),0, world.TerrainTypes.Length-1);
                        chunkData.VoxelMap[x, y, z] = new TerrainPoint((float)y - thisHeight,blockTypeInd);

                    }
                }
            }
        }

        public void SetChunkParent(GameObject parent)
        {
            _chunkObject.transform.SetParent(parent.transform);
        }

        // ====== Private Methods =======
        // ==============================
        private void CreateMeshData()
        {

            ClearMeshData();

            // Loop through each "cube" in our terrain.
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Width; z++)
                    {

                        // Pass the value into our MarchCube function.
                        MarchCube(new Vector3Int(x, y, z));

                    }
                }
            }

            BuildMesh();

        }

        private void MarchCube(Vector3Int position)
        {

            // Sample terrain values at each corner of the cube.
            float[] cube = new float[8];
            for (int i = 0; i < 8; i++)
            {

                cube[i] = SampleTerrain(position + GameData.CornerTable[i]);

            }

            // Get the configuration index of this cube.
            int configIndex = GetCubeConfiguration(cube);

            // If the configuration of this cube is 0 or 255 (completely inside the terrain or completely outside of it) we don't need to do anything.
            if (configIndex == 0 || configIndex == 255)
                return;

            // Loop through the triangles. There are never more than 5 triangles to a cube and only three vertices to a triangle.
            int edgeIndex = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int p = 0; p < 3; p++)
                {

                    // Get the current indice. We increment triangleIndex through each loop.
                    int indice = GameData.TriangleTable[configIndex, edgeIndex];

                    // If the current edgeIndex is -1, there are no more indices and we can exit the function.
                    if (indice == -1)
                        return;

                    // Get the vertices for the start and end of this edge.
                    Vector3 vert1 = position + GameData.CornerTable[GameData.EdgeIndexes[indice, 0]];
                    Vector3 vert2 = position + GameData.CornerTable[GameData.EdgeIndexes[indice, 1]];

                    Vector3 vertPosition;

                    // Get the terrain values at either end of our current edge from the cube array created above.
                    float vert1Sample = cube[GameData.EdgeIndexes[indice, 0]];
                    float vert2Sample = cube[GameData.EdgeIndexes[indice, 1]];

                    // Calculate the difference between the terrain values.
                    float difference = vert2Sample - vert1Sample;

                    // If the difference is 0, then the terrain passes through the middle.
                    if (difference == 0)
                        difference = TerrainSurface;
                    else
                        difference = (TerrainSurface - vert1Sample) / difference;

                    // Calculate the point along the edge that passes through.
                    vertPosition = vert1 + ((vert2 - vert1) * difference);

                    triangles.Add(VertForIndice(vertPosition, position));

                    edgeIndex++;

                }
            }
        }

        private int GetCubeConfiguration(float[] cube)
        {

            // Starting with a configuration of zero, loop through each point in the cube and check if it is below the terrain surface.
            int configurationIndex = 0;
            for (int i = 0; i < 8; i++)
            {

                // If it is, use bit-magic to the set the corresponding bit to 1. So if only the 3rd point in the cube was below
                // the surface, the bit would look like 00100000, which represents the integer value 32.
                if (cube[i] > TerrainSurface)
                    configurationIndex |= 1 << i;

            }

            return configurationIndex;

        }

        private float SampleTerrain(Vector3Int point)
        {

            return chunkData.VoxelMap[point.x, point.y, point.z].DistToSurface;

        }

        private int VertForIndice(Vector3 vert, Vector3Int point)
        {

            // Loop through all the vertices currently in the vertices list.
            for (int i = 0; i < vertices.Count; i++)
            {

                // If we find a vert that matches ours, then simply return this index.
                if (vertices[i] == vert)
                    return i;

            }

            // If we didn't find a match, add this vert to the list and return last index.
            vertices.Add(vert);
            TerrainType terrainType = world.TerrainTypes[chunkData.VoxelMap[point.x, point.y, point.z].TerrainTypeID];
            uvs.Add(new Vector2(terrainType.TopTexID,terrainType.SideTexID));
            return vertices.Count - 1;

        }

        private void ClearMeshData()
        {

            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
        }

        private void BuildMesh()
        {

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
        }


    }
}