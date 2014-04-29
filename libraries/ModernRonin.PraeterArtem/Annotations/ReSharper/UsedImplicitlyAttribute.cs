using System;
using System.Diagnostics.CodeAnalysis;

namespace JetBrains.Annotations
{
	/// <summary>
	/// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
	/// so this symbol will not be marked as unused (as well as by other usage inspections)
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class UsedImplicitlyAttribute : Attribute
	{
		[CanBeNull]
		public string Message { get; private set; }

		[UsedImplicitly]
		public UsedImplicitlyAttribute() 
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) 
		{}

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}

		/// <param name="message">by who?</param>
		public UsedImplicitlyAttribute([NotNull] string message) 
			: this()
		{
			Message = message;
		}

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags) : this(useKindFlags, ImplicitUseTargetFlags.Default) {}

		[SuppressMessage("Microsoft.Naming",
			"CA1726:UsePreferredTerms",
			MessageId = "Flags",
			Justification = "It is JetBrains code")]
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags) : this(ImplicitUseKindFlags.Default, targetFlags) {}

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