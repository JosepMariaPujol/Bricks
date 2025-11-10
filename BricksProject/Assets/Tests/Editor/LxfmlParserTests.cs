using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Tests
{
    [TestFixture]
    public class LxfmlParserTests
    {
        private string xml;

        [SetUp]
        public void Setup()
        {
            // Sample LXFML XML string for testing unit tests
            string xml = @"
            <LXFML versionMajor='9' versionMinor='0'>
              <Bricks>
                <Brick designID='3626' uuid='bdcc1dee-4e55-41d7-aeec-edde34e16798'>
                  <Part uuid='20436898-5eb2-4fd0-9556-6f69936c5540' designID='3626' partType='rigid' materials='1,2'>
                    <Bone uuid='4eae8f47-f49b-43cd-8b25-3d94dcff0b9a'
                          transformation='1,0,0,0,1,0,0,0,1,0,0,0' />
                  </Part>
                </Brick>
              </Bricks>
            </LXFML>";
        }
        
        [Test]
        public void ParseLxfml_ValidInput_ReturnsCorrectBrickStructure()
        {
            List<Brick> bricks = LxfmlParser.ParseLxfml(xml);

            // Validate overall structure, is not null and has one Brick
            Assert.That(bricks, Is.Not.Null);
            Assert.That(bricks.Count, Is.EqualTo(1));

            // Validate Brick properties
            Brick brick = bricks[0];
            Assert.That(brick.designID, Is.EqualTo("3626"));
            Assert.That(brick.uuid, Is.EqualTo("bdcc1dee-4e55-41d7-aeec-edde34e16798"));
            Assert.That(brick.parts.Count, Is.EqualTo(1));

            // Validate Part properties
            Part part = brick.parts[0];
            Assert.That(part.uuid, Is.EqualTo("20436898-5eb2-4fd0-9556-6f69936c5540"));
            Assert.That(part.partType, Is.EqualTo("rigid"));
            Assert.That(part.materials, Is.EqualTo("1,2"));
            Assert.That(part.bones.Count, Is.EqualTo(1));

            // Validate Bone properties
            Bone bone = part.bones[0];
            Assert.That(bone.uuid, Is.EqualTo("4eae8f47-f49b-43cd-8b25-3d94dcff0b9a"));
            Assert.That(bone.transformation, Is.EqualTo("1,0,0,0,1,0,0,0,1,0,0,0"));
        }
        [Test]
        public void Parse_Empty_ThrowsArgumentException()
        {
            // Empty string input
            Assert.Throws<ArgumentException>(() => LxfmlParser.ParseLxfml(string.Empty));
        }

        [Test]
        public void Parse_InvalidXml_ThrowsFormatException()
        {
            // Malformed XML input missing closing tag in <Part>
            string invalid = "<Brick><Part></Brick>";
            Assert.Throws<FormatException>(() => LxfmlParser.ParseLxfml(invalid));
        }

        [Test]
        public void TryParse_ReturnsFalseOnInvalidXml()
        {
            // Invalid XML string to simulate malformed input
            string invalid = "not xml";
            
            // Try to parse the invalid string using the safe TryParse method
            bool ok = LxfmlParser.TryParseLxfml(invalid, out var bricks, out var error);
            
            Assert.IsFalse(ok, "TryParseLxfml should return false for invalid XML input.");
            Assert.IsNotNull(error, "Error message should be populated when parsing fails.");
            Assert.IsEmpty(bricks, "Bricks list should be empty when parsing invalid XML.");
        }
    }
}
