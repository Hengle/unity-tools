using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Terrain2D))]
public class Terrain2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Terrain2D terrain = (Terrain2D)target;

        if (DrawDefaultInspector() && terrain.autoUpdate)
        {
            terrain.generate();
        }

        if (GUILayout.Button("Generate"))
        {
            terrain.generate();
        }
    }
}
