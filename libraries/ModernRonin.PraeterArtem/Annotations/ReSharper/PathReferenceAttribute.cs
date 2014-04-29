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
		public PathReferenceAttribute([NotNull, PathReference] string basePath)
		{
			BasePath = basePath;
		}

		[NotNull,UsedImplicitly]
		public string BasePath { get; private set; }
	}
}