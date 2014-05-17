using FluentAssertions;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.Annotations;
using ModernRonin.PraeterArtem.Reflection;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Reflection
{
    public sealed class MemberInfoExtensionsTests
    {
        [Fact]
        public void HasAttribute_When_There_AreNo_Attributes_Returns_False()
        {
            GetType()
                .HasAttribute<DataTransferObjectAttribute>()
                .Should()
                .BeFalse();
        }
        [Fact]
        public void HasAttribute_When_There_Is_Attribute_Returns_True()
        {
            typeof(WithPublicMethods).HasAttribute<UsedImplicitlyAttribute>()
                                     .Should()
                                     .BeTrue();
        }
    }
}