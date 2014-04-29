using System;

namespace ModernRonin.PraeterArtem.Annotations
{
	[MarkerClass]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class MeansNoUnitTestsNeededAttribute : Attribute
	{
	}
}