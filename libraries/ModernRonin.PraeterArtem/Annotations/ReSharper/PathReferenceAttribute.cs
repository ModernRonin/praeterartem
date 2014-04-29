using System;
using ModernRonin.PraeterArtem.Annotations;

namespace JetBrains.Annotations
{
	[Virtual(AllowedInheritanceOption.BecauseItIsDesignedSo)]
	[AttributeUsage(AttributeTargets.Parameter)]
	public class PathReferenceAttribute : Attribute
	{
		public PathReferenceAttribute() {}

		[UsedImplicitly]
		public PathReferenceAttribute([PathReference] string basePath)
		{
			BasePath = basePath;
		}

		[UsedImplicitly]
		public string BasePath { get; private set; }
	}
}