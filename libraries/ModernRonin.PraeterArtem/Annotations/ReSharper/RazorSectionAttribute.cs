using System;

namespace JetBrains.Annotations
{
	// ASP.NET MVC attributes
	// Razor attributes
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, Inherited = true)]
	public sealed class RazorSectionAttribute : Attribute {}
}