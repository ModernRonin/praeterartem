using System;
using System.Text;
using FluentAssertions;
using FluentAssertions.Common;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.Annotations;
using Xunit;
using Xunit.Extensions;
using TypeExtensions = ModernRonin.PraeterArtem.Reflection.TypeExtensions;

namespace ModernRonin.PraeterArtem.UnitTests.Reflection
{
    public sealed class TypeExtensionsTests
    {
        [Fact]
        public void HasAttribute_When_There_AreNo_Attributes_Returns_False()
        {
            typeof (TypeExtensionsTests)
                .HasAttribute<DataTransferObjectAttribute>()
                .Should()
                .BeFalse();
        }
        [Fact]
        public void HasAttribute_When_There_Is_Attribute_Returns_True()
        {
            typeof (WithPublicMethods).HasAttribute<UsedImplicitlyAttribute>()
                                      .Should()
                                      .BeTrue();
        }
        [Fact]
        public void
            HasPublicMethods_When_There_Are_Public_Methods_Returns_True()
        {
            TypeExtensions.HasPublicMethods(typeof (WithPublicMethods))
                          .Should()
                          .BeTrue();
        }
        [Fact]
        public void
            HasPublicMethods_When_There_AreNo_Public_Methods_Returns_False()
        {
            TypeExtensions.HasPublicMethods(typeof (WithoutPublicMethods))
                          .Should()
                          .BeFalse();
        }
        [Theory]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(void), "void")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(char), "char")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(uint), "uint")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(ulong), "ulong")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(ushort), "ushort")]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(sbyte), "sbyte")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(double), "double")]
        public void GetCSharpTypeName_SpecialTypes(Type type, string expected)
        {
            TypeExtensions.CSharpName(type).Should().Be(expected);
        }
        [Fact]
        public void GetCSharpTypeName_OtherType()
        {
            TypeExtensions.CSharpName(typeof(StringBuilder))
                              .Should()
                              .Be("StringBuilder");
        }
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers /* By reflection*/
            )]
        abstract class WithPublicMethods
        {
            public void A() {}
            public static void B() {}
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers /* By reflection*/
            )]
        sealed class WithoutPublicMethods : WithPublicMethods
        {
            void X() {}
            static void Y() {}
        }
    }
}