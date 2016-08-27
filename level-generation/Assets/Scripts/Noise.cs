using System;
using UnityEngine;

public static class Noise
{
    public static float[,] generateMap(int width, int height, float inverseScale, Vector2 offset = default(Vector2), int octaves = 3, float lacunarity = 2, float persistence = 0.5f)
    {
        return generateMap(width, height, inverseScale, offset, Environment.TickCount);
    }

    public static float[,] generateMap(int width, int height, float inverseScale, Vector2 offset, int octaves, float lacunarity, float persistence, out int seed)
    {
        seed = Environment.TickCount;
        return generateMap(width, height, inverseScale, offset, octaves, lacunarity, persistence, seed);
    }

    public static float[,] generateMap(int width, int height, float inverseScale, Vector2 offset, int octaves, float lacunarity, float persistence, int seed)
    {
        float[,] map = new float[width, height];
        
        System.Random rand = new System.Random(seed);

        Vector2[] centers = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            centers[i].x = (float)(rand.NextDouble() * 200000) - 100000;
            centers[i].y = (float)(rand.NextDouble() * 200000) - 100000;
        }

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                float frequency = 1;
                float amplitude = 1;
                float value = 0;

                for(int o = 0; o < octaves; o++)
                {
                    float x = (i - (width / 2f)) / inverseScale * frequency + centers[o].x + offset.x * frequency;
                    float y = (j - (height / 2f)) / inverseScale * frequency + centers[o].y + offset.y * frequency;

                    value += (Mathf.PerlinNoise(x, y) * 2 - 1) * amplitude;

                    frequency *= lacunarity;
                    amplitude *= persistence;
                }

                map[i, j] = Mathf.InverseLerp(-1, 1, value);
            }
        }

        return map;
    }
}
