using System;

namespace ModernRonin.PraeterArtem.Annotations
{
	[MarkerClass]
	[MeansNoUnitTestsNeeded]
	[MeansIntegrationTestNeeded]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class EntityFrameworkSpecificAttribute : Attribute
	{
	}
}