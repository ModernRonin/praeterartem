using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcPartialViewAttribute : PathReferenceAttribute {}
}