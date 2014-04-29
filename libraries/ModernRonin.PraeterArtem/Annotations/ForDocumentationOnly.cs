using System;

namespace ModernRonin.PraeterArtem.Annotations
{
	[MarkerClass]
	[MeansNoUnitTestsNeeded]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class ForDocumentationOnly : Attribute
	{
	}
}