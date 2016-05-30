using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenerateTexture))]
public class GenerateTextureEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateTexture gt = (GenerateTexture) target;

        if (GUILayout.Button("Random Texture", GUILayout.ExpandWidth(false)))
        {
            gt.randomTexture();
            AssetDatabase.Refresh();
        }
    }

}
