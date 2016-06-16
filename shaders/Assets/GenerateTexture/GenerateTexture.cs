using UnityEngine;
using System.IO;

public class GenerateTexture : MonoBehaviour {

    public int width = 128;
    public int height = 128;
    public float perlinScale = 20;
    public string fileName = "random.png";

    public void randomTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        PerlinHelper reds = new PerlinHelper(width, height, perlinScale);
        PerlinHelper greens = new PerlinHelper(width, height, perlinScale);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, new Color(reds[x, y], greens[x, y], 0, 1));
            }
        }

        saveTexture(texture);
    }

    private void saveTexture(Texture2D texture)
    {
        byte[] data = texture.EncodeToPNG();
        BinaryWriter writer = new BinaryWriter(File.Open(Application.dataPath + "/" + fileName, FileMode.Create));
        writer.Write(data);
        writer.Close();
    }
}
