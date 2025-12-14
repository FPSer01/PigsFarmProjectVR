using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

[CustomEditor(typeof(PigImageCreator))]
public class PigImageCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PigImageCreator creator = (PigImageCreator)target;

        GUILayout.Space(20f);

        if (GUILayout.Button("Select Save Path"))
        {
            string path = EditorUtility.SaveFolderPanel("Save File", "", "");
            
            if (!string.IsNullOrEmpty(path))
            {
                creator.SetSavePath(path);
                Debug.Log("File will be saved to: " + path);
            }
        }

        GUILayout.Space(5f);

        if (GUILayout.Button("Create images"))
        {
            creator.CreateImages();
        }
    }
}
