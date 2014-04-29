using System;

namespace JetBrains.Annotations
{
	/// <summary>
	/// Indicates that method doesn't contain observable side effects.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public sealed class PureAttribute : Attribute {}
}