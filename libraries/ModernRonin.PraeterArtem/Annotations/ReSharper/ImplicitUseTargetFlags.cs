using System;
using System.Diagnostics.CodeAnalysis;

namespace JetBrains.Annotations
{
	/// <summary>
	/// Specify what is considered to be used implicitly when marked with 
	/// <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>
	/// </summary>
	[SuppressMessage("Microsoft.Naming",
		"CA1726:UsePreferredTerms",
		MessageId = "Flags",
		Justification = "It is JetBrains code")]
	[Flags]
	public enum ImplicitUseTargetFlags
	{
		Default = Itself,
		Itself = 1,
		/// <summary>
		/// Members of entity marked with attribute are considered used
		/// </summary>
		Members = 2,
		/// <summary>
		/// Entity marked with attribute and all its members considered used
		/// </summary>
		WithMembers = Itself | Members
	}
}