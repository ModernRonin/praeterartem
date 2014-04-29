using System.Collections.Generic;
using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
	public sealed class IntegerExtensionsTests
    {
        [Fact]
        public void TimesExecute()
        {
            var count = 0;
            3.TimesExecute(() => ++count);
            count.Should().Be(3);
        }
        [Fact]
        public void TimesExecute_For_Zero()
        {
            0.TimesExecute(() => Assert.True(false));
        }
        [Fact]
        public void TimesExecute_With_IntegerAction()
        {
            var passedIntegers = new List<int>();
            3.TimesExecute(passedIntegers.Add);
            passedIntegers.ShouldAllBeEquivalentTo(new[] {0, 1, 2});
        }
        [Fact]
        public void TimesExecute_With_IntegerAction_For_Zero()
        {
            0.TimesExecute(i => Assert.True(false));
        }
    }
}