using System.Collections.Generic;
using Parser;
using UnityEngine;

public class TestLXFML : MonoBehaviour
{
    [TextArea(5, 20)]
    public string rawLXFMLText;

    public int index;

    void Start()
    {
        // Parse all bricks from the raw LXFML text
        List<Brick> bricks = LxfmlParser.ParseLxfml(rawLXFMLText);

        if (index < 0 || index >= bricks.Count)
        {
            Debug.LogWarning($"Index {index} is out of range. Total bricks: {bricks.Count}");
            return;
        }

        // Get the brick at the given index
        var brick = bricks[index];
        Debug.Log($"Brick [{index}]: DesignID={brick.designID}, UUID={brick.uuid}, Parts={brick.parts.Count}");

        // Print each part
        foreach (var part in brick.parts)
        {
            Debug.Log($"  Part: UUID={part.uuid}, DesignID={part.designID}, Type={part.partType}, Materials={part.materials}");

            // Print each bone (transformation)
            foreach (var bone in part.bones)
            {
                Debug.Log($"    Bone: UUID={bone.uuid}, Transformation={bone.transformation}");
            }
        }
    }
    
    /*void Start()
    {
        List<Brick> bricks = LxfmlParser.ParseLxfml(rawLXFMLText);

        foreach (var brick in bricks)
        {
            Debug.Log($"Brick: {brick.designID} - {brick.uuid}");
            foreach (var part in brick.parts)
            {
                Debug.Log($"  Part: {part.uuid} - {part.designID} - {part.partType} - {part.materials}");
                foreach (var bone in part.bones)
                {
                    Debug.Log($"    Bone: {bone.uuid} - {bone.transformation}");
                }
            }
        }
    }*/
}