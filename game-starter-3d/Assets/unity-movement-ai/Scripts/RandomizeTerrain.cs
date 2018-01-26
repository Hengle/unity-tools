﻿using UnityEngine;
using System.Collections;

public class RandomizeTerrain : MonoBehaviour {

    public float perlinScale = 10.0f;
    public float minHeight = 0f;
    public float maxHeight = 10f;

    public float circleCutoff = 12;

    public int numSmoothings = 3;

    public void randomize()
    {
        generateHeights(GetComponent<Terrain>(), perlinScale);
    }

    public void generateHeights(Terrain terrain, float perlinScale)
    {
        float radius = terrain.terrainData.heightmapWidth / 2;
        float innerRadius = radius - circleCutoff;

        Vector2 center = new Vector2(radius, radius);

        float minHeightPercent = minHeight / terrain.terrainData.heightmapScale.y;
        float maxHeightPercent = maxHeight / terrain.terrainData.heightmapScale.y;

        PerlinHelper ph = new PerlinHelper(terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight, perlinScale);

        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
                float distFromCenter = Vector2.Distance(new Vector2(i, k), center);

                float s = 0;

                if(distFromCenter < innerRadius)
                {
                    s = 1;
                }
                else if (distFromCenter >= innerRadius && distFromCenter < radius)
                {
                    s = (distFromCenter - innerRadius) / (radius - innerRadius);
                    s = 1 - s;
                }

                heights[i, k] = minHeightPercent + (ph[i, k] * (maxHeightPercent - minHeightPercent));
                heights[i, k] *= s;
            }
        }

        for(int i = 0; i < numSmoothings; i++)
        {
            smoothHeights(heights);
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }


    private void smoothHeights(float[,] heights)
    {
        for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int y = 0; y < heights.GetLength(1); y++)
            {
                heights[x, y] = getHeightAverage(heights, x, y);
            }
        }
    }

    private float getHeightAverage(float[,] heights, int x, int y)
    {
        float count = 0;
        float average = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < heights.GetLength(0) && j >= 0 && j < heights.GetLength(1))
                {
                    count++;
                    average += heights[i, j];
                }
            }
        }

        average /= count;

        return average;
    }
}
