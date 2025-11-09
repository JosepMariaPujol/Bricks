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
        
        // Register button callback
        selectButton?.RegisterCallback<ClickEvent>(OnSelectClick);
        importButton?.RegisterCallback<ClickEvent>(OnImportClick);

        // Hide inputs until a file is selected
        //SetUIVisible(false);
        
        //EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private void OnDisable()
    {
        selectButton?.UnregisterCallback<ClickEvent>(OnSelectClick);
        importButton?.UnregisterCallback<ClickEvent>(OnImportClick);
        
        //EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }
    
    private void OnSelectClick(ClickEvent evt)
    {
        // Open file picker
        string path = EditorUtility.OpenFilePanel("Select a text file", "", "*");

        if (!string.IsNullOrEmpty(path))
        {
            filePath = path;
            ReadFile(path);

            // Update UI values
            selectInput.value = filePath;
            contentInput.value = fileContents;
            
            //SetUIVisible(true);
        }
    }
    
    private void OnImportClick(ClickEvent evt)
    {
        Debug.Log("Import button clicked. Implement import functionality here.");
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
    
    private void SetUIVisible(bool visible)
    {
        var display = visible ? DisplayStyle.Flex : DisplayStyle.None;

        selectLabel.style.display = display;
        selectInput.style.display = display;
        contentLabel.style.display = display;
        contentInput.style.display = display;
        importButton.style.display = display;
    }
    
    //Automatically reset UI when exiting Play Mode
    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode || state == PlayModeStateChange.EnteredEditMode)
        {
            ResetRuntimeUI();
        }
    }

    private void ResetRuntimeUI()
    {
        filePath = "";
        fileContents = "";
        if (selectInput != null) selectInput.value = "";
        if (contentInput != null) contentInput.value = "";
        SetUIVisible(false);
        Debug.Log("🔄 UI reset after exiting Play Mode");
    }
}