using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator lg = (LevelGenerator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            lg.generate();
            EditorUtility.SetDirty(target);
        }
    }
}
