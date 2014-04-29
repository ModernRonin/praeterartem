using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcActionAttribute : Attribute
	{
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }
		public AspMvcActionAttribute() {}

		public AspMvcActionAttribute(string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}
	}
}