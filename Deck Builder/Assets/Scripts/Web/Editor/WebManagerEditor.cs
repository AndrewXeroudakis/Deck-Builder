using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(WebManager))]
public class WebManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WebManager serverAPI = (WebManager)target;
        GUILayout.Space(20f);
        GUI.color = Color.cyan;
        if (GUILayout.Button("Open Data Folder"))
        {
            SaveLoadManager.OpenSaveFolder(serverAPI.dataFolderName);
        }
    }
}