using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorLand : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap};
    public DrawMode drawMode;

    public int mapWidth, mapHeight,seed;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public Vector2 offSet;

    public bool autoUpdate;

    public TerrainType[] regions;
    public void GenerateMapLand()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth,mapHeight,seed,noiseScale,octaves,persistance,lacunarity,offSet);
        Color[] colourMap = new Color[mapWidth*mapHeight];
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                float currentHeight = noiseMap[x,y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplayLand display = FindObjectOfType<MapDisplayLand>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGeneratorLand.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGeneratorLand.TextureFromColourMap(colourMap,mapWidth,mapHeight));
        }
    }

    private void OnValidate()
    {
        if(mapWidth < 1)
        {
            mapWidth = 1;
        }
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }
}
[System.Serializable]
public struct TerrainType
{
    public float height;
    public Color color;
    public string name;
}
