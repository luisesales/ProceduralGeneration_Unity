using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PerlinNoise : MonoBehaviour
{
    // Start is called before the first frame update
    public int width, height;
    private Renderer renderer;

    public float scale,offsetX,offsetY;


    private void Start()
    {
        offsetX = Random.Range(0f,99999f);
        offsetY = Random.Range(0f, 99999f);
    }

    void Update()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                texture.SetPixel(i, j, CalculateColor(i, j));
            }
        }
        texture.Apply();
        return texture;
    }



    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
;