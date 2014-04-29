using System;

namespace ModernRonin.PraeterArtem.Annotations
{
	/// <summary>Static class that is has only constants, static read-only fields, and nested classes</summary>
	[MarkerClass]
	[MeansNoUnitTestsNeeded]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class NamespaceObjectAttribute : Attribute
	{
	}
}