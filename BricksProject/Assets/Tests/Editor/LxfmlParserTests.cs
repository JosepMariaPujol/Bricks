using NUnit.Framework;
using System;
using System.Linq;

namespace Parser.Tests
{
    [TestFixture]
    public class LxfmlParserTests
    {
        [Test]
        public void Parse_ValidXml_ReturnsBricks()
        {
            string xml = @"<Bricks>
  <Brick uuid=""b1"" designID=""3001"">
    <Part uuid=""p1"" designID=""3001c"" partType=""normal"" materials=""1"">
      <Bone uuid=""bn1"" transformation=""1 0 0 0 1 0 0 0 1"" />
    </Part>
  </Brick>
</Bricks>";

            var bricks = LxfmlParser.ParseLxfml(xml);

            Assert.IsNotNull(bricks);
            Assert.AreEqual(1, bricks.Count);
            var brick = bricks.First();
            Assert.AreEqual("b1", brick.uuid);
            Assert.AreEqual("3001", brick.designID);
            Assert.AreEqual(1, brick.parts.Count);
            var part = brick.parts[0];
            Assert.AreEqual("p1", part.uuid);
            Assert.AreEqual("3001c", part.designID);
            Assert.AreEqual("normal", part.partType);
            Assert.AreEqual("1", part.materials);
            Assert.AreEqual(1, part.bones.Count);
            Assert.AreEqual("bn1", part.bones[0].uuid);
            Assert.AreEqual("1 0 0 0 1 0 0 0 1", part.bones[0].transformation);

            // Validate ParseTransformation returns the expected numeric values
            /*var numbers = part.bones[0].ParseTransformation();
            Assert.AreEqual(9, numbers.Length);
            Assert.AreEqual(1.0, numbers[0]);
            Assert.AreEqual(1.0, numbers[4]);
            Assert.AreEqual(1.0, numbers[8]);*/
        }

        [Test]
        public void Parse_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => LxfmlParser.ParseLxfml(string.Empty));
        }

        [Test]
        public void Parse_InvalidXml_ThrowsFormatException()
        {
            string invalid = "<Brick><Part></Brick>";
            Assert.Throws<FormatException>(() => LxfmlParser.ParseLxfml(invalid));
        }

        [Test]
        public void TryParse_ReturnsFalseOnInvalidXml()
        {
            string invalid = "not xml";
            bool ok = LxfmlParser.TryParseLxfml(invalid, out var bricks, out var error);
            Assert.IsFalse(ok);
            Assert.IsNotNull(error);
            Assert.IsEmpty(bricks);
        }

        [Test]
        public void Parse_NamespacedXml_ShouldParse()
        {
            // Default namespace is present; parser uses local-name() so it should still find elements
            string nsXml = @"<Bricks xmlns=""http://example.com/lxfml"">
  <Brick uuid=""b2"" designID=""3002"">
    <Part uuid=""p2"" designID=""3002c"" partType=""normal"" materials=""2"">
      <Bone uuid=""bn2"" transformation=""0.5 0 0 0 0.5 0 0 0 0.5"" />
    </Part>
  </Brick>
</Bricks>";

            var ok = LxfmlParser.TryParseLxfml(nsXml, out var bricks, out var error);
            Assert.IsTrue(ok, error);
            Assert.AreEqual(1, bricks.Count);
            var bone = bricks[0].parts[0].bones[0];
            /*var nums = bone.ParseTransformation();
            Assert.AreEqual(9, nums.Length);
            Assert.AreEqual(0.5, nums[0]);
            Assert.AreEqual(0.5, nums[4]);
            Assert.AreEqual(0.5, nums[8]);*/
        }
    }
}
