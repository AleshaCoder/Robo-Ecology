using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CameraShot
{
    //private static Camera _shotCamera;
    //[SerializeField] private string _textureName;

    [MenuItem("CONTEXT/Camera/Save Screen")]
    private static void Shot()
    {
        var shotCamera = Selection.activeGameObject.GetComponent(typeof(Camera)) as Camera;
        int width = shotCamera.pixelWidth;
        int height = shotCamera.pixelHeight;
        Texture2D texture = new Texture2D(width, height);

        RenderTexture targetTexture = RenderTexture.GetTemporary(width, height);

        shotCamera.targetTexture = targetTexture;
        shotCamera.Render();

        RenderTexture.active = targetTexture;

        Rect rect = new Rect(0, 0, width, height);
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();

        string path = EditorUtility.SaveFilePanel("Save Screenshot", "Assets/", texture.name, "png");            
        if (string.IsNullOrEmpty(path))
            return;

        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
        Debug.Log("Saved to " + path);
        Debug.Log("OK");
        shotCamera.targetTexture = null;
        shotCamera.Render();
    }
}
