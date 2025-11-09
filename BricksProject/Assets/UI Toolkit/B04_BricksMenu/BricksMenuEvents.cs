using UnityEngine;
using UnityEngine.UIElements;
using Parser;
using System.Collections.Generic;

public class BricksMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label headerLabel;
    private Toggle treeListToggle;
    private Button previousButton;
    private Button nextButton;

    [SerializeField]
    private ImportMenuEvents importMenuEvents;
    
    [SerializeField]
    private Transform brickPiece;
    
    private List<Brick> bricksCache;
    private VisualElement infoContainer; // Container for brick info

    private int index = 0;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        
        // Query UI elements by name
        headerLabel = root.Q<Label>("Header");
        previousButton = root.Q<Button>("PreviousButton");
        nextButton = root.Q<Button>("NextButton");
        treeListToggle = root.Q<Toggle>("TreeListToggle");

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
        (treeListToggle.value ? (System.Action<int>)ShowBrickTreeAtIndex : ShowBrickTableAtIndex).Invoke(index);
    }

    private void OnNextClick(ClickEvent evt)
    {
        EnsureBricksCache();
        index = (index == bricksCache.Count - 1) ? 0 : index + 1;
        (treeListToggle.value ? (System.Action<int>)ShowBrickTreeAtIndex : ShowBrickTableAtIndex).Invoke(index);
    }

    private void EnsureBricksCache()
    {
        if (bricksCache == null || bricksCache.Count == 0)
        {
            bricksCache = LxfmlParser.ParseLxfml(importMenuEvents.fileContents);
        }
    }

    private void ShowBrickTreeAtIndex(int i)
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
    
    private void ShowBrickTableAtIndex(int i)
    {
        index = i;
        infoContainer.Clear(); // Remove old labels
        var brick = bricksCache[i];

        // Header
        if (headerLabel != null)
            headerLabel.text = $"[{i}] - Brick {brick.designID} [{brick.uuid}]";

        // Column titles
        var headerRow = new VisualElement();
        headerRow.style.flexDirection = FlexDirection.Row;
        headerRow.style.justifyContent = Justify.SpaceBetween;
        headerRow.style.paddingLeft = 5;
        headerRow.style.paddingRight = 5;
        headerRow.style.marginBottom = 5;
        headerRow.style.unityFontStyleAndWeight = FontStyle.Bold;

        headerRow.Add(new Label("Part ID") { style = { flexGrow = 1, minWidth = 300 } });
        headerRow.Add(new Label("Part Type") { style = { flexGrow = 1, minWidth = 300 } });
        headerRow.Add(new Label("Materials") { style = { flexGrow = 1, minWidth = 300 } });
        headerRow.Add(new Label("Bone UUID") { style = { flexGrow = 2, minWidth = 900 } });
        headerRow.Add(new Label("Transformation") { style = { flexGrow = 3, minWidth = 2000 } });

        infoContainer.Add(headerRow);

        // Separator
        var separator = new VisualElement();
        separator.style.height = 1;
        separator.style.backgroundColor = Color.gray;
        separator.style.marginBottom = 5;
        infoContainer.Add(separator);

        // Table rows
        foreach (var part in brick.parts)
        {
            foreach (var bone in part.bones)
            {
                AddTableRow(part.designID, part.partType, part.materials, bone.uuid, bone.transformation);
            }
        }
    }

    /// <summary>
    /// Adds a single row to the infoContainer with proper spacing
    /// </summary>
    private void AddTableRow(string partId, string partType, string materials, string boneUuid, string transformation)
    {
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.SpaceBetween;
        row.style.paddingLeft = 5;
        row.style.paddingRight = 5;
        row.style.marginBottom = 24;       // spacing between rows
        row.style.marginTop = 24;       // spacing between rows
        row.style.minHeight = 24;         // ensures row is tall enough for text

        row.Add(new Label(partId) { style = { flexGrow = 1, minWidth = 300 } });
        row.Add(new Label(partType) { style = { flexGrow = 1, minWidth = 300 } });
        row.Add(new Label(materials) { style = { flexGrow = 1, minWidth = 300 } });
        row.Add(new Label(boneUuid) { style = { flexGrow = 2, minWidth = 900 } });
        row.Add(new Label(transformation) { style = { flexGrow = 5, minWidth = 2000, whiteSpace = WhiteSpace.Normal } });
        
        infoContainer.Add(row);
    }
}
