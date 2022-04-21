using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class MapDisplay : MonoBehaviour
    {

		public Renderer textureRender;
		public bool autoUpdate;
		public LevelSettings levelSettings;

		[SerializeField] private int _seed;
		[SerializeField] private int _worldSizeInChunksX;
		[SerializeField] private int _worldSizeInChunksY;
		[SerializeField] private float _scale;
		[SerializeField] private int _octaves;
		[SerializeField, Range(0, 1)] private float _persistance;
		[SerializeField] private float _lacunarity;
		[SerializeField] private Vector2 _offset;



		private void Awake()
        {
            
        }

		public void GenerateRenderer()
        {
			Clear();
			textureRender = GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<Renderer>();
			textureRender.gameObject.transform.position = new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth/2, 0, levelSettings.WorldSizeInChunksY * GameData.ChunkWidth/2);
			textureRender.gameObject.transform.rotation = Quaternion.identity;
			textureRender.gameObject.transform.RotateAround(new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth / 2, 0, levelSettings.WorldSizeInChunksY * GameData.ChunkWidth / 2), transform.up, 180);
			textureRender.gameObject.transform.SetParent(transform);
			
			textureRender.transform.localScale = new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth / 10,
				1,
				levelSettings.WorldSizeInChunksY * GameData.ChunkWidth / 10);
		}

        public void DrawNoiseMap(float[,] noiseMap = null)
		{
			GenerateRenderer();

			if(noiseMap == null)
			noiseMap = Noise.GenerateNoise(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth + 1,
				levelSettings.WorldSizeInChunksY * GameData.ChunkWidth + 1,
				levelSettings.Seed,
				levelSettings.NoiseSettings.Scale,
				levelSettings.NoiseSettings.Octaves,
				levelSettings.NoiseSettings.Persistance,
				levelSettings.NoiseSettings.Lacunarity,
				levelSettings.NoiseSettings.Offset
				);



			int width = noiseMap.GetLength(0);
			int height = noiseMap.GetLength(1);

			Texture2D texture = new Texture2D(width, height);

			Color[] colourMap = new Color[width * height];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
				}
			}
			texture.SetPixels(colourMap);
			texture.Apply();

			textureRender.sharedMaterial.mainTexture = texture;
		}

		public void Clear()
        {
			if (textureRender != null)
				DestroyImmediate(textureRender.gameObject);
		}

        private void OnValidate()
        {
			levelSettings.Seed = _seed;

			if (_worldSizeInChunksX < 1)
				_worldSizeInChunksX = 1;
			levelSettings.WorldSizeInChunksX = _worldSizeInChunksX;

			if (_worldSizeInChunksY < 1)
				_worldSizeInChunksY = 1;
			levelSettings.WorldSizeInChunksY = _worldSizeInChunksY;

			levelSettings.NoiseSettings.Scale = _scale;

			if (_octaves < 1)
			{
				_octaves = 1;
			}
			levelSettings.NoiseSettings.Octaves = _octaves;

			if (_lacunarity < 1)
			{
				_lacunarity = 1;
			}
			levelSettings.NoiseSettings.Lacunarity = _lacunarity;

			if (_persistance < 0)
			{
				_persistance = 0;
			}
			if (_persistance > 1)
			{
				_persistance = 1;
			}
			levelSettings.NoiseSettings.Persistance = _persistance;

			levelSettings.NoiseSettings.Offset = _offset;

        }
    }
}
