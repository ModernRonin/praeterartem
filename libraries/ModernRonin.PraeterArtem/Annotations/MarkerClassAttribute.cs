using System;

namespace ModernRonin.PraeterArtem.Annotations
{
	/// <summary> Allows ctor-s only </summary>
	[MarkerClass]
	[MeansNoUnitTestsNeeded]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class MarkerClassAttribute : Attribute
	{
	}
}