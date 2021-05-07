using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorMap
{
    public Color color;
    public float height;
}

public class PerlinTextureGenerator : MonoBehaviour
{


    public int height = 256;
    public int width = 256;
    public float scale = 10f;
    private float xOffset = 100f;
    private float yOffset = 100f;

    public ColorMap[] colors;

    private void OnValidate()
    {
        Renderer renderer = this.GetComponent<Renderer>();
        xOffset = Random.Range(0, 9999);
        yOffset = Random.Range(0, 9999);
        renderer.sharedMaterial.mainTexture = GenerateTexture();
    }
    void Start()
    {

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = new Color(0, 0, 0, 0);
                int colorIndex = 0;
                float sample = Mathf.PerlinNoise((float)x / width * scale + xOffset, (float)y / height * scale + yOffset);
                while (colorIndex < colors.Length)
                {
                    if (sample <= colors[colorIndex].height)
                    {
                        color += colors[colorIndex].color;
                        break;
                    }
                    colorIndex++;
                }
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }
}
