using System;
using FluentAssertions;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests
{
    public sealed class LocalTimeGiverTests
    {
        [Fact]
        public void Now_Gives_Now()
        {
            new LocalTimeGiver().Now.Should().BeCloseTo(DateTime.Now);
        }
    }
}