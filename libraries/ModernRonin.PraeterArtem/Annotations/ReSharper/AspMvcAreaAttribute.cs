using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcAreaAttribute : PathReferenceAttribute
	{
		[CanBeNull]
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		[UsedImplicitly]
		public AspMvcAreaAttribute() {}

		public AspMvcAreaAttribute([NotNull] string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}
	}
}