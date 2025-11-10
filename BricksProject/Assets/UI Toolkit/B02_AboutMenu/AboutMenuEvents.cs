using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class AboutMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button backButton;
    private TextField aboutInput;
    private string aboutString = 
        "BricksProject is a Unity-based tool designed to import, parse, and visualize LXFML data. \n" +
        "It reads Digital Designer model files (.LXFML), extracts information about bricks, parts, \n" +
        "and assemblies, and presents them interactively using the Unity UI Toolkit interface. \n\n" +
        "This project demonstrates a workflow for interpreting structured XML data formats in Unity, \n" +
        "as part of an ongoing exploration into procedural content, custom import pipelines, \n " +
        "and UI-driven data visualization. \n\n" +
        "Developed and maintained by Josep Maria Pujol.";

    
    
    public Transform mainMenuCameraTransform;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query UI elements by name
        aboutInput = root.Q<TextField>("AboutInput");
        backButton = root.Q<Button>("BackButton");
        
        aboutInput.value = aboutString;
        
        // Register button callback
        backButton?.RegisterCallback<ClickEvent>(OnBackClick);
    }

    private void OnDisable()
    {
        backButton?.UnregisterCallback<ClickEvent>(OnBackClick);
    }
    
    private void OnBackClick(ClickEvent evt)
    {
        if (mainMenuCameraTransform == null)
        {
            Debug.LogWarning("Main Menu Camera Transform is not assigned.");
            return;
        }

        // Move the main camera to the target transform
        Camera.main.transform.position = mainMenuCameraTransform.position;
        Camera.main.transform.rotation = mainMenuCameraTransform.rotation;
    }
}
