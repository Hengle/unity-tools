    using UnityEngine;
using System.Collections;

public class Terrain2D : MonoBehaviour
{
    public int width;
    public int height;
    public float inverseScale;

    public bool randomSeed;
    public int seed;

    public bool autoUpdate;

    void Start()
    {
        generate();
    }

    public void generate()
    {
        float[,] heights;

        if(randomSeed)
        {
            heights = Noise.generateMap(width, height, inverseScale, out seed);
        }
        else
        {
            heights = Noise.generateMap(width, height, inverseScale, seed);
        }

        Color[] colors = new Color[width * height];
        for(int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                colors[(j * width) + i] = Color.Lerp(Color.black, Color.white, heights[i, j]);
            }
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colors);
        texture.Apply();

        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
        transform.localScale = new Vector3(((float)width / height), 1f, 1f);
    }

    void OnValidate()
    {
        if(width < 1)
        {
            width = 1;
        }

        if(height < 1)
        {
            height = 1;
        }

        if(inverseScale <= 0)
        {
            inverseScale = 0.0001f;
        }
    }
}
