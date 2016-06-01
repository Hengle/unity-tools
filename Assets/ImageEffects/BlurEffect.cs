using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour {

    public Material mat;

    [Tooltip("The number of times the screen texture's width and height will be scaled down in half.")]
    public int numberHalfSize;

    [Tooltip("The number of times the image is run through the blur shader.")]
    public int iterations;


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mat != null)
        {
            int width = src.width >> numberHalfSize;
            int height = src.height >> numberHalfSize;

            RenderTexture rt1 = RenderTexture.GetTemporary(width, height);
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            RenderTexture rtSwap;

            Graphics.Blit(src, rt1);

            for (int i = 0; i < iterations; i++)
            {
                Graphics.Blit(rt1, rt2, mat);
                rtSwap = rt1;
                rt1 = rt2;
                rt2 = rtSwap;
            }

            Graphics.Blit(rt1, dest);
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }
    }
}
