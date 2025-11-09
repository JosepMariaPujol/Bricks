using UnityEngine;
using UnityEngine.UIElements;
using Parser;
using System.Collections.Generic;

public class BricksMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label headerLabel;
    private Button previousButton;
    private Button nextButton;

    [SerializeField]
    private ImportMenuEvents importMenuEvents;

    private List<Brick> bricksCache;
    private VisualElement infoContainer; // Container for brick info

    private int index = -1;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        
        // Query UI elements by name
        headerLabel = root.Q<Label>("Header");
        previousButton = root.Q<Button>("PreviousButton");
        nextButton = root.Q<Button>("NextButton");

        // Register button callback
        previousButton?.RegisterCallback<ClickEvent>(OnPreviousClick);
        nextButton?.RegisterCallback<ClickEvent>(OnNextClick);

        // Create a container for labels if not in UXML
        infoContainer = root.Q<VisualElement>("BrickInfoLabelContainer");
        if (infoContainer == null)
        {
            infoContainer = new VisualElement();
            infoContainer.name = "BrickInfoLabelContainer";
            infoContainer.style.height = 300; // Fixed height
            infoContainer.style.width = 420; // Slightly wider than labels
            infoContainer.style.flexDirection = FlexDirection.Column;

            infoContainer.style.color = Color.white;

            root.Add(infoContainer);
        }
    }

    private void OnDisable()
    {
        previousButton?.UnregisterCallback<ClickEvent>(OnPreviousClick);
        nextButton?.UnregisterCallback<ClickEvent>(OnNextClick);
    }

    private void OnPreviousClick(ClickEvent evt)
    {
        EnsureBricksCache();
        
        index = (index == 0) ? bricksCache.Count - 1 : index - 1;
        ShowBrickAtIndex(index);
    }

    private void OnNextClick(ClickEvent evt)
    {
        EnsureBricksCache();
        
        index = (index == bricksCache.Count - 1) ? 0 : index + 1;
        ShowBrickAtIndex(index);
    }

    private void EnsureBricksCache()
    {
        if (bricksCache == null || bricksCache.Count == 0)
        {
            bricksCache = LxfmlParser.ParseLxfml(importMenuEvents.fileContents);
        }
    }

    private void ShowBrickAtIndex(int i)
    {
        index = i;
        
        infoContainer.Clear(); // Remove old labels

        var brick = bricksCache[i];
        
        // Brick label
        if (headerLabel != null)
            //headerLabel.text = $"Brick [{i}]  -  DesignID: {brick.designID}, UUID: {brick.uuid}";
            headerLabel.text = $"[{i}] - Brick {brick.designID} [{brick.uuid}]";

        infoContainer.Add(new Label($"Parts: {brick.parts.Count}:"));

        foreach (var part in brick.parts)
        {
            // Part label
            infoContainer.Add(new Label($" Part {part.designID} ({part.partType}, materials: {part.materials})"));

            // Spacing
            infoContainer.Add(new Label($""));
            
            if (part.bones != null)
            {
                foreach (var bone in part.bones)
                {
                    // Bone label
                    
                    infoContainer.Add(new Label($"  Bone {bone.uuid}"));
                    infoContainer.Add(new Label($"    Transformation: ({bone.transformation}"));
                }
            }
        }
    }
}
