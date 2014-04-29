using System;
using System.Diagnostics.CodeAnalysis;

namespace JetBrains.Annotations
{
	/// <summary>
	/// Should be used on attributes and causes ReSharper to not mark symbols marked with such attributes as unused (as well as by other usage inspections)
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class MeansImplicitUseAttribute : Attribute
	{
		[UsedImplicitly]
		public MeansImplicitUseAttribute() : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags) : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags) : this(ImplicitUseKindFlags.Default, targetFlags) { }

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }
		/// <summary>
		/// Gets value indicating what is meant to be used
		/// </summary>
		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}