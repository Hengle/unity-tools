    using UnityEngine;
using System.Collections;

public class Terrain2D : MonoBehaviour
{
    private Renderer r;

    // Use this for initialization
    void Start()
    {
        r = GetComponent<Renderer>();

        int width = 100;
        int height = 100;

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

    // Update is called once per frame
    void Update()
    {

    }
}
