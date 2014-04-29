using System;

namespace JetBrains.Annotations
{
	/// <summary>
	/// This attribute is intended to mark publicly available API which should not be removed and so is treated as used.
	/// </summary>
	[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
	public sealed class PublicApiAttribute : Attribute
	{
		public PublicApiAttribute() {}
		// ReSharper disable UnusedParameter.Local
		public PublicApiAttribute(string comment) {}
		// ReSharper restore UnusedParameter.Local
	}
}