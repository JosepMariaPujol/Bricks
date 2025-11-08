using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ImportMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button selectButton;
    private Button importButton;
    
    private TextField selectInput;
    private TextField contentInput;
    
    private string filePath = "";
    private string fileContents = "";
    
    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        
        selectButton = uiDocument.rootVisualElement.Q("SelectButton") as Button;
        selectButton.RegisterCallback<ClickEvent>(OnPlaySelectClick);
    }

    private void OnDisable()
    {
        selectButton.UnregisterCallback<ClickEvent>(OnPlaySelectClick);
    }

    private void OnPlaySelectClick(ClickEvent evt)
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
        
        Debug.Log("You pressed the Import Button");
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
