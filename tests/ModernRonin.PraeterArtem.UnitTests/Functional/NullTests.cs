using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using NUnit.Framework;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
    [TestFixture]
    public class NullTests
    {
        [Test]
        public void Enumerable_Is_Empty()
        {
            Null.Enumerable<int>().Should().BeEmpty();
        }
        [Test]
        public void Action_Does_Nothing()
        {
            // actually, I don't see any way to prove that the following call does nothing ;)
            Null.Action()();
        }
        [Test]
        public void Action_With_Argument_Does_Nothing()
        {
            // actually, I don't see any way to prove that the following call does nothing ;)
            Null.Action<int>()(3);
        }
    }
}