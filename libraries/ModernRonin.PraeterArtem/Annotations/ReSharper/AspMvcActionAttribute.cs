using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcActionAttribute : Attribute
	{
		[CanBeNull]
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }
		public AspMvcActionAttribute() {}

		public AspMvcActionAttribute([CanBeNull] string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}
	}
}