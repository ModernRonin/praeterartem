using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace ModernRonin.PraeterArtem.UnitTests
{
    public sealed class DateTimeExtensionsTests
    {
        public static IEnumerable<object[]> WindowsToUnixTestCases
        {
            get
            {
                yield return
                    new object[]
                    {new DateTime(2013, 6, 8, 18, 0, 0), 1370707200};
                yield return
                    new object[] {new DateTime(1970, 1, 3, 4, 5, 6), 183906};
                yield return
                    new object[]
                    {new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), 0};
            }
        }
        [Theory]
        [PropertyData("WindowsToUnixTestCases")]
        public void SpecificLocalTimeToUnixTime(DateTime windows, int unix)
        {
            Console.WriteLine(windows);
            Console.WriteLine(windows.IsDaylightSavingTime());
            Console.WriteLine(windows.ToUniversalTime());
            Console.WriteLine(
                              windows.ToUniversalTime()
                                     .IsDaylightSavingTime());
            windows.ToUnixTime().Should().Be(unix);
        }
        [Theory]
        [InlineData(740125382)]
        public void RoundTrip(int input)
        {
            var regular = DateTimeExtensions.FromUnixTimeToUtc(input);
            var output = regular.ToUnixTime();
            output.Should().Be(input);
        }
        [Fact]
        public void RoundTripWithSpecificTimeZone()
        {
            var input = new DateTime(2012, 2, 15, 13, 33, 17,
                DateTimeKind.Local);
            var unix = input.ToUnixTime();
            var output =
                DateTimeExtensions.FromUnixTimeToUtc(unix).ToLocalTime();
            output.Should().Be(input);
        }
        [Theory]
        [PropertyData("WindowsToUnixTestCases")]
        public void SpecificUnixTimeToLocalTime(DateTime windows, int unix)
        {
            var output = DateTimeExtensions.FromUnixTimeToUtc(unix);
            if (windows.Kind != DateTimeKind.Utc)
                windows = windows.ToUniversalTime();
            output.Should().Be(windows);
        }
    }
}