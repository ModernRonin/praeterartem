using System.Xml.Linq;
using FluentAssertions;
using ModernRonin.PraeterArtem.Xml;
using NUnit.Framework;

namespace ModernRonin.PraeterArtem.UnitTests.Xml
{
    [TestFixture]
    public class XElementEqualityComparerTests
    {
        XElementEqualityComparer mUnderTest;
        [SetUp]
        public void Setup()
        {
            mUnderTest = new XElementEqualityComparer();
        }
        [Test]
        public void ChangedAttributeOrderDoesNotMatter()
        {
            var lhs =
                XDocument.Parse("<Alpha bravo=\"1\" charlie=\"2\"/>").Root;
            var rhs =
                XDocument.Parse("<Alpha charlie=\"2\" bravo=\"1\"/>").Root;
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
        [Test]
        public void SeparateCloseTagOrNotDoesNotMatter()
        {
            var lhs = XDocument.Parse("<Alpha/>").Root;
            var rhs = XDocument.Parse("<Alpha></Alpha>").Root;
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
        [Test]
        public void NestedStructure()
        {
            var lhs =
                XDocument.Parse(
                                "<Alpha bravo=\"1\" charlie=\"2\"><Delta echo=\"3\" foxtrott=\"4\"/><Golf>5</Golf></Alpha>")
                         .Root;
            var rhs =
                XDocument.Parse(
                                "<Alpha charlie=\"2\" bravo=\"1\"><Golf>5</Golf><Delta foxtrott=\"4\" echo=\"3\"></Delta></Alpha>")
                         .Root;
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
    }
}