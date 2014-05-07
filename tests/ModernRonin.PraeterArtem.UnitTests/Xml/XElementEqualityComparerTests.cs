using System.Xml.Linq;
using FluentAssertions;
using ModernRonin.PraeterArtem.UnitTests.Properties;
using ModernRonin.PraeterArtem.Xml;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Xml
{
    public sealed class XElementEqualityComparerTests
    {
        readonly XElementEqualityComparer mUnderTest;
        public XElementEqualityComparerTests()
        {
            mUnderTest = new XElementEqualityComparer();
        }
        [Fact]
        public void ChangedAttributeOrderDoesNotMatter()
        {
            var lhs = XElement.Parse("<Alpha bravo=\"1\" charlie=\"2\"/>");
            var rhs = XElement.Parse("<Alpha charlie=\"2\" bravo=\"1\"/>");
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
        [Fact]
        public void Different_Attribute_Values_Result_In_NotEqual()
        {
            var lhs = XElement.Parse("<Alpha bravo=\"1\" charlie=\"2\"/>");
            var rhs = XElement.Parse("<Alpha bravo=\"2\" charlie=\"1\"/>");
            mUnderTest.Equals(lhs, rhs).Should().BeFalse();
        }
        [Fact]
        public void
            Different_Attribute_Values_With_Nested_Node_Result_In_NotEqual()
        {
            var lhs = XElement.Parse("<Alpha bravo=\"1\"><Charlie/></Alpha>");
            var rhs = XElement.Parse("<Alpha bravo=\"2\"><Charlie/></Alpha>");
            mUnderTest.Equals(lhs, rhs).Should().BeFalse();
        }
        [Fact]
        public void Regression_1()
        {
            var lhs = XElement.Parse(Resources.Regression1Left);
            var rhs = XElement.Parse(Resources.Regression1Right);
            mUnderTest.Equals(lhs, rhs).Should().BeFalse();
        }
        [Fact]
        public void SeparateCloseTagOrNotDoesNotMatter()
        {
            var lhs = XElement.Parse("<Alpha/>");
            var rhs = XElement.Parse("<Alpha></Alpha>");
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
        [Fact]
        public void NestedStructure()
        {
            var lhs =
                XElement.Parse("<Alpha bravo=\"1\" charlie=\"2\">" +
                               "<Delta echo=\"3\" foxtrot=\"4\"/>" +
                               "<Golf>5</Golf>" + "</Alpha>");
            var rhs =
                XElement.Parse("<Alpha charlie=\"2\" bravo=\"1\">" +
                               "<Golf>5</Golf>" +
                               "<Delta foxtrot=\"4\" echo=\"3\"></Delta>" +
                               "</Alpha>");
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
    }
}