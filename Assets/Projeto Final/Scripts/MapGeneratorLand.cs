using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorLand : MonoBehaviour
{
    public int mapWidth, mapHeight,seed;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public Vector2 offSet;

    public bool autoUpdate;
    public void GenerateMapLand()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth,mapHeight,seed,noiseScale,octaves,persistance,lacunarity,offSet);

        MapDisplayLand display = FindObjectOfType<MapDisplayLand>();
        display.DrawNoiseMap(noiseMap);
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
