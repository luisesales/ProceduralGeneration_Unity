using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth,int mapHeight,int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offSet)
    {
        float[,] noiseMap = new float[mapWidth,mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffSets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offSetX = prng.Next(-100000, 100000) + offSet.x;
            float offSetY = prng.Next(-100000, 100000) + offSet.y;
            octaveOffSets[i] = new Vector2(offSetX, offSetY);
        }
        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        float maxNoiseHeight = float.MinValue, minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f, halfHeight = mapHeight/2f;
        for(int x = 0; x < mapWidth;x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                float amplitude = 1, frequency = 1, noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffSets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffSets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 -1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                    
                }
                if( noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if( noiseHeight < minNoiseHeight ) minNoiseHeight = noiseHeight;
                noiseMap[x, y] = noiseHeight;
            }
        }
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

}
