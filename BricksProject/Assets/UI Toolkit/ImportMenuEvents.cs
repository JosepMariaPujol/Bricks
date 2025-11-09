using System;
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

    public Transform bricksMenuCameraTransform;

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
        if (importButton != null)
            importButton.style.display = DisplayStyle.None;

        // Register button callback
        selectButton?.RegisterCallback<ClickEvent>(OnSelectClick);
        importButton?.RegisterCallback<ClickEvent>(OnImportClick);
    }

    private void OnDisable()
    {
        selectButton?.UnregisterCallback<ClickEvent>(OnSelectClick);
        importButton?.UnregisterCallback<ClickEvent>(OnImportClick);
    }

    private void OnSelectClick(ClickEvent evt)
    {
        // Open file picker
        string path = EditorUtility.OpenFilePanel("Select a text file", "", "*");

        if (!string.IsNullOrEmpty(path))
        {
            filePath = path;
            ReadFile(path);

            importButton.style.display = DisplayStyle.Flex;

            // Update UI values
            selectInput.value = filePath;
            contentInput.value = fileContents;
        }
    }

    private void OnImportClick(ClickEvent evt)
    {
        if (bricksMenuCameraTransform == null)
        {
            Debug.LogWarning("Bricks Menu Camera Transform is not assigned.");
            return;
        }

        // Move the main camera to the target transform
        Camera.main.transform.position = bricksMenuCameraTransform.position;
        Camera.main.transform.rotation = bricksMenuCameraTransform.rotation;
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
}