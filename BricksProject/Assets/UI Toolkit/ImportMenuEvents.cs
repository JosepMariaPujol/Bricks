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
    private Label selectLabel;
    private Label contentLabel;
    
    private string filePath = "";
    private string fileContents = "";

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query UI elements by name
        selectButton = root.Q<Button>("SelectButton");
        importButton = root.Q<Button>("ImportButton");
        selectInput = root.Q<TextField>("SelectInput");
        contentInput = root.Q<TextField>("ContentInput");
        selectLabel = root.Q<Label>("SelectLabel");
        contentLabel = root.Q<Label>("ContentLabel");
        
        // Hide inputs until a file is selected
        selectInput.style.display = DisplayStyle.None;
        contentInput.style.display = DisplayStyle.None;
        selectLabel.style.display = DisplayStyle.None;
        contentLabel.style.display = DisplayStyle.None;
        importButton.style.display = DisplayStyle.None;

        // Register button callback
        selectButton.RegisterCallback<ClickEvent>(OnSelectClick);
        importButton.RegisterCallback<ClickEvent>(OnImportClick);

    }

    private void OnDisable()
    {
        selectButton?.UnregisterCallback<ClickEvent>(OnSelectClick);
        importButton?.UnregisterCallback<ClickEvent>(OnImportClick);
    }

    private void OnImportClick(ClickEvent evt)
    {
        Debug.Log("Import button clicked. Implement import functionality here.");
    }
    
    private void OnSelectClick(ClickEvent evt)
    {
        // Open file picker
        string path = EditorUtility.OpenFilePanel("Select a text file", "", "*");

        if (!string.IsNullOrEmpty(path))
        {
            filePath = path;
            ReadFile(path);

            // Show input fields after file selection
            selectLabel.style.display = DisplayStyle.Flex;
            selectInput.style.display = DisplayStyle.Flex;
            contentLabel.style.display = DisplayStyle.Flex;
            contentInput.style.display = DisplayStyle.Flex;
            importButton.style.display = DisplayStyle.Flex;

            // Update UI values
            selectInput.value = filePath;
            contentInput.value = fileContents;
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