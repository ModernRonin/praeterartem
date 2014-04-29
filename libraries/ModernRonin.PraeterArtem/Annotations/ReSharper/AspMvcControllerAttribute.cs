using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcControllerAttribute : Attribute
	{
		[CanBeNull]
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }
		public AspMvcControllerAttribute() {}

		public AspMvcControllerAttribute([CanBeNull]string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}
	}
}