using UnityEngine;

public static class Noise
{
    public static float[,] foo()
    {
        return new float[10, 10];
    }

    public static float[,] foo(out int seed)
    {
        seed = 0;
        return new float[10, 10];
    }

    public static float[,] generateMap(int width, int height, int seed)
    {
        float[,] map = new float[width, height];

        System.Random rand = new System.Random(seed);

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {

            }
        }

        return map;
    }
}
