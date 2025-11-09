using UnityEngine;
using UnityEngine.UIElements;
using Parser;
using System.Collections.Generic;

public class BricksMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button previousButton;
    private Button nextButton;

    private int index;

    [SerializeField]
    private ImportMenuEvents importMenuEvents;

    private List<Brick> bricksCache;

    private VisualElement infoContainer; // Container for brick info

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

        // Create a container for labels if not in UXML
        infoContainer = root.Q<VisualElement>("InfoContainer");
        if (infoContainer == null)
        {
            infoContainer = new VisualElement();
            infoContainer.name = "InfoContainer";
            infoContainer.style.flexDirection = FlexDirection.Column;
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
        infoContainer.Add(new Label($"Brick [{i}] - DesignID: {brick.designID}, UUID: {brick.uuid}, Parts: {brick.parts.Count}"));
        //Debug.Log($"Brick [{i}]: DesignID={brick.designID}, UUID={brick.uuid}, Parts={brick.parts.Count}");

        foreach (var part in brick.parts)
        {
            // Part label (indented)
            var partLabel = new Label($"  Part - UUID: {part.uuid}, DesignID: {part.designID}, Type: {part.partType}, Materials: {part.materials}");
            Debug.Log($"  Part: UUID={part.uuid}, DesignID={part.designID}, Type={part.partType}, Materials={part.materials}");

            infoContainer.Add(partLabel);

            if (part.bones != null)
            {
                foreach (var bone in part.bones)
                {
                    // Bone label (more indented)
                    var boneLabel = new Label($"    Bone - UUID: {bone.uuid}, Transformation: {bone.transformation}");
                    //Debug.Log($"    Bone: UUID={bone.uuid}, Transformation={bone.transformation}");

                    infoContainer.Add(boneLabel);
                }
            }
        }
    }
}
