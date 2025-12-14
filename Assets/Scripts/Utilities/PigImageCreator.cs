using System;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Класс утилита для создания картинок свиней из RenderTexture в PNG
/// </summary>
public class PigImageCreator : MonoBehaviour
{
    [SerializeField] private List<CameraData> cameraData;
    [Space]
    [TextArea(2, 10)]
    [SerializeField] private string fileName;
    [TextArea(2, 10)]
    [SerializeField] private string savePath;

    /// <summary>
    /// Создать картинки и сохранить по пути на диск
    /// </summary>
    public void CreateImages()
    {
        if (fileName.IsNullOrEmpty() || savePath.IsNullOrEmpty())
        {
            Debug.LogError("Нет названия файлов или пути сохранения");
            return;
        }

        for (int i = 0; i < cameraData.Count; i++)
        {
            try 
            {
                CameraData data = cameraData[i];

                RenderTexture.active = data.RenderTexture;

                Texture2D tex = ConvertRenderTextureToTexture2D(data.Camera, data.RenderTexture);

                byte[] pngData = tex.EncodeToPNG();

                string pngName = $"{fileName}_{i}.png";
                string fullPath = savePath + "/" + pngName;

                System.IO.File.WriteAllBytes(fullPath, pngData);
                DestroyImmediate(tex);

                Debug.Log($"Создана картинка {pngName} и сохранена по пути {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Картинки созданы");
    }

    public void SetSavePath(string path)
    {
        savePath = path;
    }

    private Texture2D ConvertRenderTextureToTexture2D(Camera cam, RenderTexture rTex)
    {
        cam.Render();

        // Создаем Texture2D
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        return tex;
    }

    [Serializable]
    public struct CameraData
    {
        public RenderTexture RenderTexture;
        public Camera Camera;
    }
}
