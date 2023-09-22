using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    private int[,] map;
    public int mapWidht, mapHeight;
    //public float noiseScale;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map = new int[mapWidht, mapHeight];
        RandomFillMap();
        SmoothMap();
    }

    int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;
        for(int neighbourX= x -1; neighbourX <= x+1; neighbourX++)
        {
            for(int neighbourY = y -1; neighbourY <= y+1; neighbourY++)
            {
                if (neighbourX == 0 && neighbourX < mapWidht && neighbourY >= 0 && neighbourY < mapHeight)
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void SmoothMap()
    {
        for(int i = 0; i < mapWidht; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(i,j);
                if(neighbourWallTiles > 4)
                {
                    map[i, j] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    map[i, j] = 0;
                }
            }
        }
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            //seed = (Random.Range(0, mapWidht * mapHeight) * Time.time).ToString();
            seed = Time.time.ToString();
        }
        System.Random prng = new System.Random(seed.GetHashCode());
        for(int i = 0; i < mapWidht; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                if (i == 0 || i == mapWidht - 1 || j == 0 || j == mapHeight - 1) 
                    map[i, j] = 1;
                else
                    map[i, j] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(map != null)
        {
            for (int i = 0; i < mapWidht; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    Gizmos.color = (map[i, j] == 1)? Color.black : Color.white;
                    Vector3 pos = new Vector3(-mapWidht / 2 + i + .5f, 0, -mapHeight / 2 + j + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
    
        
    
}
