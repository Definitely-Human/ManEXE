using ManExe.Core;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.Noise
{
    public static class Noise
    {
        public static float[,] GenerateNoise(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, AnimationCurve heightCurve = null)
        {
			float[,] noiseMap = new float[mapWidth, mapHeight];

			System.Random prng = new System.Random(seed);
			Vector2[] octaveOffsets = new Vector2[octaves];

			float amplitude = 1;
			float frequency = 1;

			for (int i = 0; i < octaves; i++)
			{
				float offsetX = prng.Next(-10000, 10000) * scale  + offset.x ;
				float offsetY = prng.Next(-10000, 10000) * scale - offset.y ;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);

			}

			float maxLocalNoiseHeight = float.MinValue;
			float minLocalNoiseHeight = float.MaxValue;

			float halfWidth = mapWidth / 2f;
			float halfHeight = mapHeight / 2f;


			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{

					amplitude = 1;
					frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
						float sampleY = (y - halfHeight - octaveOffsets[i].y) / scale * frequency;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight)
					{
						maxLocalNoiseHeight = noiseHeight;
					}
					if (noiseHeight < minLocalNoiseHeight)
					{
						minLocalNoiseHeight = noiseHeight;
					}
					noiseMap[x, y] = noiseHeight;

					
				}
			}

			
				for (int y = 0; y < mapHeight; y++)
				{
					for (int x = 0; x < mapWidth; x++)
					{
						noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
					}
				}
			return noiseMap; ;
        }
        public static float[,] GenerateNoise(LevelSettings levelSettings)
        {
            float[,] noiseMap = GenerateNoise(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth + 1,
                levelSettings.WorldSizeInChunksY * GameData.ChunkWidth + 1,
                levelSettings.Seed,
                levelSettings.NoiseSettings.Scale,
                levelSettings.NoiseSettings.Octaves,
                levelSettings.NoiseSettings.Persistance,
                levelSettings.NoiseSettings.Lacunarity,
                levelSettings.NoiseSettings.Offset,
                levelSettings.NoiseSettings.HeightCurve);

            return noiseMap;
        }

        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int terrainHeightRange, int baseTerrainHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, AnimationCurve heightCurve = null)
        {
            float[,] noiseMap = GenerateNoise(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset, heightCurve);

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = terrainHeightRange * noiseMap[x, y] + baseTerrainHeight;
                }
            }

            return noiseMap;
        }
        public static float[,] GenerateNoiseMap(LevelSettings levelSettings)
        {
            float[,] noiseMap = GenerateNoiseMap(levelSettings.WorldSizeInChunksX * GameData.ChunkWidth + 1,
                levelSettings.WorldSizeInChunksY * GameData.ChunkWidth + 1,
                levelSettings.TerrainHeightRange,
                levelSettings.BaseTerrainHeight,
                levelSettings.Seed,
                levelSettings.NoiseSettings.Scale,
                levelSettings.NoiseSettings.Octaves,
                levelSettings.NoiseSettings.Persistance,
                levelSettings.NoiseSettings.Lacunarity,
                levelSettings.NoiseSettings.Offset,
                levelSettings.NoiseSettings.HeightCurve);

            return noiseMap;
        }

    }
}