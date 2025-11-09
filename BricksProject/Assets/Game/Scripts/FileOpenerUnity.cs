using UnityEditor;
using UnityEngine;
using System.IO;

public class FileReaderUtility : EditorWindow
{
    private string filePath = "";
    private string fileContents = "";

    [MenuItem("Tools/File Reader")]
    public static void ShowWindow()
    {
        GetWindow<FileReaderUtility>("File Reader");
    }

    private void OnGUI()
    {
        GUILayout.Label("File Reader Tool", EditorStyles.boldLabel);

        if (GUILayout.Button("Select File"))
        {
            // Opens a native file picker
            string path = EditorUtility.OpenFilePanel(
                "Select a text file",
                "",
                "*"
            );

            if (!string.IsNullOrEmpty(path))
            {
                filePath = path;
                ReadFile(path);
            }
        }

        if (!string.IsNullOrEmpty(filePath))
        {
            GUILayout.Label("Selected File:", EditorStyles.label);
            EditorGUILayout.TextField(filePath);

            GUILayout.Space(10);

            GUILayout.Label("File Contents:", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(fileContents, GUILayout.Height(200));
        }
    }

    private void ReadFile(string path)
    {
        try
        {
            fileContents = File.ReadAllText(path);
            Debug.Log($"✅ Successfully read file: {path}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Failed to read file: {ex.Message}");
        }
    }
}