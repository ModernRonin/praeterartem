using FluentAssertions;
using FluentAssertions.Common;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.Annotations;
using Xunit;
using TypeExtensions = ModernRonin.PraeterArtem.Reflection.TypeExtensions;

namespace ModernRonin.PraeterArtem.UnitTests.Reflection
{
	public sealed class TypeExtensionsTests
	{
		[Fact]
		public void HasAttribute_When_There_AreNo_Attributes_Should_Return_False()
		{
			typeof (TypeExtensionsTests)
				.HasAttribute<DataTransferObjectAttribute>()
				.Should().BeFalse();
		}
		[Fact]
		public void HasAttribute_When_There_Is_Attribute_Should_Return_True()
		{
			typeof (WithPublicMethods)
				.HasAttribute<UsedImplicitlyAttribute>()
				.Should().BeTrue();
		}

		[Fact]
		public void HasPublicMethods_When_There_Are_Public_Methods_Should_Return_True()
		{
			TypeExtensions.HasPublicMethods(typeof(WithPublicMethods)).Should().BeTrue();
		}

		[Fact]
		public void HasPublicMethods_When_There_AreNo_Public_Methods_Should_Return_False()
		{
			TypeExtensions.HasPublicMethods(typeof (WithoutPublicMethods)).Should().BeFalse();
		}
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers /* By reflection*/)]
		abstract class WithPublicMethods
		{
			public void A() { }
			public static void B() { }
		}
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers /* By reflection*/)]
		sealed class WithoutPublicMethods : WithPublicMethods
		{
			private void X() { }
			private static void Y() { }
		}
	}
}
