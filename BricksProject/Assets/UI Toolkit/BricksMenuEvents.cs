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
        EnsureBricksCache();
        
        if (index == 0)
            index = bricksCache.Count - 1;
        else
            index--;
        
        ShowBrickAtIndex(index);
    }

    private void OnNextClick(ClickEvent evt)
    {
        EnsureBricksCache();
        
        if (index == bricksCache.Count - 1)
            index = 0;
        else
            index++;
        
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

        var brick = bricksCache[i];
        Debug.Log($"Brick [{i}]: DesignID={brick.designID}, UUID={brick.uuid}, Parts={brick.parts.Count}");

        foreach (var part in brick.parts)
        {
            Debug.Log($"  Part: UUID={part.uuid}, DesignID={part.designID}, Type={part.partType}, Materials={part.materials}");

            if (part.bones != null)
            {
                foreach (var bone in part.bones)
                {
                    Debug.Log($"    Bone: UUID={bone.uuid}, Transformation={bone.transformation}");
                }
            }
        }
    }
}
