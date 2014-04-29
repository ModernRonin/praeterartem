using System;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Annotations
{
	/// <summary>Marks DTO classes</summary>
	[MarkerClass]
	[MeansNoUnitTestsNeeded]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
	public sealed class DataTransferObjectAttribute : Attribute
	{
	}
}