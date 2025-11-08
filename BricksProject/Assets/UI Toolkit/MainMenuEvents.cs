using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        
        startButton = uiDocument.rootVisualElement.Q("StartButton") as Button;
        startButton.RegisterCallback<ClickEvent>(OnPlayStartClick);
        
        Button quitButton = uiDocument.rootVisualElement.Q("QuitButton") as Button;
        quitButton.RegisterCallback<ClickEvent>(OnPlayQuitClick);
    }

    private void OnDisable()
    {
        startButton.UnregisterCallback<ClickEvent>(OnPlayStartClick);
        startButton.UnregisterCallback<ClickEvent>(OnPlayQuitClick);
    }

    private void OnPlayStartClick(ClickEvent evt)
    {
        
        
        Debug.Log("You pressed the Start Button");
    }
    
    
    private void OnPlayQuitClick(ClickEvent evt)
    {
        // Stops Play Mode in the Editor
        EditorApplication.isPlaying = false; 

        Debug.Log("You pressed the Quit Button");
    }
}
