using System;
using UnityEngine;

public static class Noise
{
    public static float[,] generateMap(int width, int height)
    {
        return generateMap(width, height, Environment.TickCount);
    }

    public static float[,] generateMap(int width, int height, out int seed)
    {
        seed = Environment.TickCount;
        return generateMap(width, height, seed);
    }

    public static float[,] generateMap(int width, int height, int seed)
    {
        float[,] map = new float[width, height];
        
        System.Random rand = new System.Random(seed);

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                map[i, j] = (float) rand.NextDouble();
            }
        }

        return map;
    }
}
