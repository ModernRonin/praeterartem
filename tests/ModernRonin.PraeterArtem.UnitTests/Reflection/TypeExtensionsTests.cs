using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.Annotations;
using ModernRonin.PraeterArtem.Reflection;
using Xunit;
using Xunit.Extensions;

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
            typeof (WithPublicMethods).HasPublicMethods().Should().BeTrue();
        }
        [Fact]
        public void
            HasPublicMethods_When_There_AreNo_Public_Methods_Returns_False()
        {
            typeof (WithoutPublicMethods).HasPublicMethods()
                                         .Should()
                                         .BeFalse();
        }
        [Theory]
        [InlineData(typeof (string), "string")]
        [InlineData(typeof (void), "void")]
        [InlineData(typeof (object), "object")]
        [InlineData(typeof (bool), "bool")]
        [InlineData(typeof (char), "char")]
        [InlineData(typeof (int), "int")]
        [InlineData(typeof (uint), "uint")]
        [InlineData(typeof (long), "long")]
        [InlineData(typeof (ulong), "ulong")]
        [InlineData(typeof (short), "short")]
        [InlineData(typeof (ushort), "ushort")]
        [InlineData(typeof (byte), "byte")]
        [InlineData(typeof (sbyte), "sbyte")]
        [InlineData(typeof (float), "float")]
        [InlineData(typeof (double), "double")]
        public void CSharpName_SpecialTypes(Type type, string expected)
        {
            type.CSharpName().Should().Be(expected);
        }
        [Fact]
        public void CSharpName_OtherType()
        {
            typeof (StringBuilder).CSharpName().Should().Be("StringBuilder");
        }
        [Fact]
        public void PrettyName_For_CSharp_Type_Returns_CSharpName()
        {
            typeof (Int32).PrettyName().Should().Be("int");
        }
        [Fact]
        public void
            PrettyName_For_Generic_Type_With_CSharpType_As_TypeParameter()
        {
            typeof (List<Int32>).PrettyName().Should().Be("List<int>");
        }
        [Fact]
        public void PrettyName_With_Multiple_Mixed_TypeParameters()
        {
            typeof (Action<Int32, string, StringBuilder>).PrettyName()
                                                         .Should()
                                                         .Be(
                                                             "Action<int, string, StringBuilder>");
        }
        [Fact]
        public void PrettyName_For_Nested_Generic_Type()
        {
            typeof (
                Action
                    <Int32, Dictionary<TypeExtensionsTests, LinkedList<bool>>,
                        StringBuilder>).PrettyName()
                                       .Should()
                                       .Be(
                                           "Action<int, Dictionary<TypeExtensionsTests, LinkedList<bool>>, StringBuilder>");
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