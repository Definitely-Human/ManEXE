using ManExe.Core;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.Noise
{
    public class MapDisplay : MonoBehaviour
    {

		public Renderer textureRender;
		public LevelSettings levelSettings;
		public bool AutoUpdate;

		private void Awake()
        {
            
        }

		public void GenerateRenderer()
        {
			Clear();
			textureRender = GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<Renderer>();
			textureRender.gameObject.transform.position = new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth/2, 0, levelSettings.WorldSizeInChunksZ * GameData.ChunkWidth/2);
			textureRender.gameObject.transform.rotation = Quaternion.identity;
			textureRender.gameObject.transform.RotateAround(new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth / 2, 0, levelSettings.WorldSizeInChunksZ * GameData.ChunkWidth / 2), transform.up, 180);
			textureRender.gameObject.transform.SetParent(transform);
			
			textureRender.transform.localScale = new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth / 10,
				1,
				levelSettings.WorldSizeInChunksZ * GameData.ChunkWidth / 10);
		}

        public void DrawNoiseMap(float[,] noiseMap = null)
		{
			if (textureRender == null)
				return;
			textureRender.gameObject.transform.position = new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth / 2, 0, levelSettings.WorldSizeInChunksZ * GameData.ChunkWidth / 2);
			textureRender.transform.localScale = new Vector3(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth / 10,
				1,
				levelSettings.WorldSizeInChunksZ * GameData.ChunkWidth / 10);

			noiseMap = Noise.GenerateNoise(levelSettings);



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

		public void OnValuesChange()
        {
            if (!Application.isPlaying)
            {
				DrawNoiseMap();
            }
        }

		public void Clear()
        {

			if (textureRender != null)
				DestroyImmediate(textureRender.gameObject);
		}

        private void OnValidate()
        {

			if(levelSettings != null)
            {
				levelSettings.OnValuesUpdated -= OnValuesChange;
				levelSettings.OnValuesUpdated += OnValuesChange;
            }
			if(levelSettings.NoiseSettings != null)
			{
				levelSettings.NoiseSettings.OnValuesUpdated -= OnValuesChange;
				levelSettings.NoiseSettings.OnValuesUpdated += OnValuesChange;
			}
		}
    }
}
