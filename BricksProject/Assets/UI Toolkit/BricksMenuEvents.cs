using UnityEngine;
using UnityEngine.UIElements;
using Parser;
using System.Collections.Generic;

public class BricksMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button previousButton;
    private Button nextButton;

    private int index = 0;
    
    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query UI elements by name
        previousButton = root.Q<Button>("PreviousButton");
        nextButton = root.Q<Button>("NextButton");
        
        // Register button callback
        previousButton?.RegisterCallback<ClickEvent>(OnPreviousClick);
        nextButton?.RegisterCallback<ClickEvent>(OnNextClick);
    }

    private void OnDisable()
    {
        previousButton?.UnregisterCallback<ClickEvent>(OnPreviousClick);
        nextButton?.UnregisterCallback<ClickEvent>(OnNextClick);
    }

    private void OnPreviousClick(ClickEvent evt)
    {
        
    }
    private void OnNextClick(ClickEvent evt)
    {
        
    }
}

    



