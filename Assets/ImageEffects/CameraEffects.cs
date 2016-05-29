using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraEffects : MonoBehaviour {

    public Material mat;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(mat != null)
        {
            Graphics.Blit(src, dest, mat);
        }
    }
}
