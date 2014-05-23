using System;
using FluentAssertions;
using ModernRonin.PraeterArtem.Scheduling;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Scheduling
{
    public sealed class AThrottlingConfigurableTests
    {
        [Fact]
        public void Construction()
        {
            var underTest = new Testable();
            underTest.ShouldBeEquivalentTo(
                                           new
                                           {
                                               Duration =
                                                   TimeSpan.FromSeconds(1),
                                               MaximumRequestCountPerDuration
                                                   = 1
                                           });
        }

        class Testable : AThrottlingConfigurable {}
    }
}