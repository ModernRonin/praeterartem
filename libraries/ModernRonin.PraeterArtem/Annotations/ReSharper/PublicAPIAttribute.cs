using System;

namespace JetBrains.Annotations
{
	/// <summary>
	/// This attribute is intended to mark publicly available API which should not be removed and so is treated as used.
	/// </summary>
	[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
	[AttributeUsage(AttributeTargets.All)]
	public sealed class PublicApiAttribute : Attribute
	{
		[NotNull]
		public string Comment { get; private set; }
		public PublicApiAttribute() {}
		public PublicApiAttribute([NotNull] string comment)
		{
			Comment = comment;
		}
	}
}