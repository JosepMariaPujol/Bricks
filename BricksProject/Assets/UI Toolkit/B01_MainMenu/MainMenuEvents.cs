using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;
    private Button aboutButton;
    private Button quitButton;

    public Transform aboutMenuCameraTransform;
    public Transform importMenuCameraTransform;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query UI elements by name
        startButton = root.Q<Button>("StartButton");
        aboutButton = root.Q<Button>("AboutButton");
        quitButton = root.Q<Button>("QuitButton");

        // Register button callback
        startButton.RegisterCallback<ClickEvent>(OnPlayStartClick);
        aboutButton.RegisterCallback<ClickEvent>(OnAboutClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitClick);
    }
    

    private void OnDisable()
    {
        startButton.UnregisterCallback<ClickEvent>(OnPlayStartClick);
        
        aboutButton.UnregisterCallback<ClickEvent>(OnAboutClick);
        
        quitButton.UnregisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnPlayStartClick(ClickEvent evt)
    {
        if (importMenuCameraTransform == null)
        {
            Debug.LogWarning("Import Menu Camera Transform is not assigned.");
            return;
        }

        // Move the main camera to the target transform
        Camera.main.transform.position = importMenuCameraTransform.position;
        Camera.main.transform.rotation = importMenuCameraTransform.rotation;
    }
    
    private void OnAboutClick(ClickEvent evt)
    {
        if (aboutMenuCameraTransform == null)
        {
            Debug.LogWarning("About Menu Camera Transform is not assigned.");
            return;
        }

        // Move the main camera to the target transform
        Camera.main.transform.position = aboutMenuCameraTransform.position;
        Camera.main.transform.rotation = aboutMenuCameraTransform.rotation;
    }
    
    private void OnQuitClick(ClickEvent evt)
    {
        // Stops Play Mode in the Editor
        EditorApplication.isPlaying = false; 

        Debug.Log("You pressed the Quit Button");
    }
}
