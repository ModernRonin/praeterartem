using System.Collections.Generic;
using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
    public sealed class CollectionExtensionsTests
    {
        [Fact]
        public void RemoveWhere()
        {
            var collection = new List<string>
                             {
                                 "alpha",
                                 "bravo",
                                 "charlie",
                                 "delta",
                                 "echo",
                                 "foxtrott",
                                 "golf",
                                 "hotel",
                                 "india",
                                 "juliet",
                                 "kilo"
                             };
            collection.RemoveWhere(s => s.Contains("a"));
            collection.ShouldAllBeEquivalentTo(new[]
                                               {
                                                   "echo", "foxtrott", "golf",
                                                   "hotel", "juliet", "kilo"
                                               });
        }
    }
}