using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField] MeshRenderer noiseObjRederer;
    // Assetsより下から書く
    [SerializeField] string directoryName;
    [SerializeField] string textureName;

    [SerializeField] Vector2Int textureSize;

    [SerializeField] Texture2D texture;

    [SerializeField] Material fBmMaterial;
    [SerializeField] Material clearMaterial;
    [SerializeField] Material copyMaterial;

    [SerializeField] NoiseDataPack pack;

    // 個別のプロジェクトに移行して、githubに挙げる


    private void OnDestroy()
    {
        Destroy(texture);
    }

    void Test()
    {
        DofBm();
        texture.Apply();
    }

    void DofBm()
    {
        // 重ね合わせるテクスチャはRGBAFloatで用意
        Texture2D result = new Texture2D(textureSize.x, textureSize.y, TextureFormat.RGBAFloat, false);
        Texture2D noiseTexture = new Texture2D(textureSize.x, textureSize.y, TextureFormat.RGBA32, false);
        BlitToOtherWithReadable(result, result, clearMaterial, destFormat: RenderTextureFormat.ARGBFloat);

        fBmMaterial.SetTexture("_BaseTex", result);

        float fbmMax = 0.0f;
        int i = 0;
        foreach (var data in pack.datas)
        {
            if (data.active == false) continue;
            float f = Mathf.Pow(0.5f, i);
            float scale = data.scale * Mathf.Pow(2.0f, i);
            float amplitude = data.amplitude;

            data.material.SetVector("_Offset", data.offset);
            data.material.SetFloat("_Scale", scale);
            data.material.SetFloat("_Amplitude", amplitude);
            // 真っ白テクスチャにBlit
            BlitToOtherWithReadable(noiseTexture, noiseTexture, data.material);

            //fとノイズを入力して、resultに入力する;
            fBmMaterial.SetFloat("_Scale", f);
            BlitToOtherWithReadable(noiseTexture, result, fBmMaterial, destFormat: RenderTextureFormat.ARGBFloat);
            fbmMax += f;
            i++;
        }


        Texture2D tmp = new Texture2D(textureSize.x, textureSize.y, TextureFormat.RGBA32, false);
        BlitToOtherWithReadable(tmp, tmp, clearMaterial);
        // 最後にresultをfbmMaxで割って、RGBA32に入れる
        fBmMaterial.SetFloat("_Scale", 1f / fbmMax);
        fBmMaterial.SetTexture("_BaseTex", tmp);
        BlitToOtherWithReadable(result, texture, fBmMaterial);

        DestroyImmediate(result);
        DestroyImmediate(noiseTexture);
        DestroyImmediate(tmp);
    }

    public void BlitToOtherWithReadable(Texture2D source, Texture2D dest, Material mat, RenderTextureFormat destFormat = RenderTextureFormat.ARGB32)
    {
        var tmpBuffer = RenderTexture.GetTemporary(source.width, source.height, 0, destFormat, RenderTextureReadWrite.Default);

        Graphics.Blit(source, tmpBuffer, mat, -1);
        Graphics.CopyTexture(tmpBuffer, dest);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpBuffer;
        dest.ReadPixels(new Rect(0, 0, tmpBuffer.width, tmpBuffer.height), 0, 0);
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpBuffer);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NoiseGenerator))]
    public class NoiseGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NoiseGenerator myTarget = (NoiseGenerator)target;

            if (GUILayout.Button("Initialize"))
            {
                DestroyImmediate(myTarget.texture);
                Vector2Int size  = myTarget.textureSize;
                if (size.x == 0.0f) size.x = 256;
                if (size.y == 0.0f) size.y = 256;
                Texture2D texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.filterMode = FilterMode.Point;
                myTarget.texture = texture;
                myTarget.noiseObjRederer.sharedMaterial.mainTexture = texture;
                myTarget.BlitToOtherWithReadable(texture, texture, myTarget.clearMaterial);
            }

            if (GUILayout.Button("Test"))
            {
                // shaderでやる
                myTarget.Test();
            }

            if (GUILayout.Button("Output Texture"))
            {
                string storagePath = $"{Application.dataPath}/{myTarget.directoryName}/{myTarget.textureName}.png";

                var png = myTarget.texture.EncodeToPNG();
                File.WriteAllBytes(storagePath, png);
            }
        }
    }
#endif
}
