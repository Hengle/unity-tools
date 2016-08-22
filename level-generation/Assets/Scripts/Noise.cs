using System;
using UnityEngine;

public static class Noise
{
    public static float[,] generateMap(int width, int height, float inverseScale)
    {
        return generateMap(width, height, inverseScale, Environment.TickCount);
    }

    public static float[,] generateMap(int width, int height, float inverseScale, out int seed)
    {
        seed = Environment.TickCount;
        return generateMap(width, height, inverseScale, seed);
    }

    public static float[,] generateMap(int width, int height, float inverseScale, int seed)
    {
        float[,] map = new float[width, height];
        
        System.Random rand = new System.Random(seed);

        Vector2 center = new Vector2();
        center.x = (float) (rand.NextDouble() * 200000) - 100000;
        center.y = (float) (rand.NextDouble() * 200000) - 100000;

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                float x = (i - (width / 2f)) / inverseScale + center.x;
                float y = (j - (height / 2f)) / inverseScale + center.y;
                
                map[i, j] = Mathf.PerlinNoise(x, y);
            }
        }

        return map;
    }
}
