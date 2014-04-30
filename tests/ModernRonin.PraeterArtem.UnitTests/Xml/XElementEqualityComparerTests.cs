using System.Xml.Linq;
using FluentAssertions;
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
        public void SeparateCloseTagOrNotDoesNotMatter()
        {
			var lhs = XElement.Parse("<Alpha/>");
			var rhs = XElement.Parse("<Alpha></Alpha>");
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
        [Fact]
        public void NestedStructure()
        {
			var lhs = XElement.Parse(
				"<Alpha bravo=\"1\" charlie=\"2\">" +
					"<Delta echo=\"3\" foxtrot=\"4\"/>" +
					"<Golf>5</Golf>" +
				"</Alpha>");
			var rhs = XElement.Parse(
				"<Alpha charlie=\"2\" bravo=\"1\">" +
					"<Golf>5</Golf>" +
					"<Delta foxtrot=\"4\" echo=\"3\"></Delta>" +
				"</Alpha>");
            mUnderTest.Equals(lhs, rhs).Should().BeTrue();
        }
    }
}