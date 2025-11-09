using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class AboutMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button backButton;

    public Transform mainMenuCameraTransform;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query UI elements by name
        backButton = root.Q<Button>("BackButton");
        
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
            Debug.LogWarning("⚠️ Main Menu Camera Transform is not assigned.");
            return;
        }

        // Move the main camera to the target transform
        Camera.main.transform.position = mainMenuCameraTransform.position;
        Camera.main.transform.rotation = mainMenuCameraTransform.rotation;
    }
}
