using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public float tileSize = 16f;

    public TextAsset mapFile;

    public Transform[] prefabs;


    // Use this for initialization
    void Start()
    {
        
    }

    public void generate()
    {
        destroyChildren();

        float halfTileSize = tileSize / 2f;

        int[,] data = getMapData();

        for(int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                Transform p = prefabs[data[i, j]];
                if(p != null)
                {
                    Instantiate(p, new Vector2(halfTileSize + i * tileSize, halfTileSize + j * tileSize), Quaternion.identity, transform);
                }
            }
        }
    }

    private void destroyChildren()
    {
        while (transform.childCount > 0)
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private int[,] getMapData()
    {
        int[][] rows = mapFile.text.Split('\n').Select(r => r.Trim().Split(' ').Select(int.Parse).ToArray()).ToArray();

        int[,] data = new int[rows[0].Length, rows.Length];

        for (int j = 0; j < rows.Length; j++)
        {
            for (int i = 0; i < rows[0].Length; i++)
            {
                data[i, rows.Length - 1 - j] = rows[j][i];
            }
        }

        return data;
    }
}
