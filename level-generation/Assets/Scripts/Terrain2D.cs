    using UnityEngine;
using System.Collections;

public class Terrain2D : MonoBehaviour
{
    public int width;
    public int height;

    public bool autoUpdate;

    public void generate()
    {
        Renderer r = GetComponent<Renderer>();

        float[,] heights = Noise.generateMap(width, height);

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

        r.sharedMaterial.mainTexture = texture;
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
    }
}
