using System;
using System.Collections.Generic;
using System.Xml;

namespace Parser
{
    [Serializable]
    public class Brick
    {
        public string uuid;
        public string designID;
        public List<Part> parts = new List<Part>();
    }

    [Serializable]
    public class Part
    {
        public string uuid;
        public string designID;
        public string partType;
        public string materials;
        public List<Bone> bones = new List<Bone>();
    }

    [Serializable]
    public class Bone
    {
        public string uuid;
        public string transformation;
    }

    public class LxfmlParser
    {
        /// <summary>
        /// Parses raw LXFML text and returns a list of bricks properties.
        /// Throws ArgumentException for null/empty input and FormatException for invalid XML.
        /// </summary>
        public static List<Brick> ParseLxfml(string rawLxfml)
        {
            var bricks = new List<Brick>();

            if (string.IsNullOrWhiteSpace(rawLxfml))
                throw new ArgumentException("rawLXFML must not be null or empty", nameof(rawLxfml));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rawLxfml);
            
            XmlNodeList brickNodes = xmlDoc.GetElementsByTagName("Brick");
            foreach (XmlNode brickNode in brickNodes)
            {
                if (brickNode == null) continue;

                Brick brick = new Brick();
                brick.uuid = GetAttribute(brickNode,"uuid");
                brick.designID = GetAttribute(brickNode,"designID");

                foreach (XmlNode partNode in brickNode.SelectNodes("Part"))
                {
                    if (partNode == null) continue;

                    Part part = new Part();
                    part.uuid = GetAttribute(partNode,"uuid");
                    part.designID = GetAttribute(partNode,"designID");
                    part.partType = GetAttribute(partNode,"partType");
                    part.materials = GetAttribute(partNode,"materials");

                    foreach (XmlNode boneNode in partNode.SelectNodes("Bone"))
                    {
                        if (boneNode == null) continue;

                        Bone bone = new Bone();
                        bone.uuid = GetAttribute(boneNode,"uuid");
                        bone.transformation = GetAttribute(boneNode,"transformation");

                        part.bones.Add(bone);
                    }

                    brick.parts.Add(part);
                }

                bricks.Add(brick);
            }

            return bricks;
        }

        /// <summary>
        /// Try-parse variant that doesn't throw and returns an error message on failure.
        /// </summary>
        public static bool TryParseLxfml(string rawLxfml, out List<Brick> bricks, out string errorMessage)
        {
            try
            {
                bricks = ParseLxfml(rawLxfml);
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                bricks = new List<Brick>();
                errorMessage = ex.Message;
                return false;
            }
        }

        private static string GetAttribute(XmlNode node, string name)
        {
            return node?.Attributes?[name]?.Value;
        }
    }
}
