using System;
using FluentAssertions;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests
{
    public sealed class UtcTimeGiverTests
    {
        [Fact]
        public void Now_Gives_UtcNow()
        {
            new UtcTimeGiver().Now.Should().BeCloseTo(DateTime.UtcNow);
        }
    }
}