using System;

namespace ModernRonin.PraeterArtem.Annotations
{
	[MarkerClass]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class VirtualAttribute : Attribute
	{
		public AllowedInheritanceOption Option { get; set; }
		public VirtualAttribute() { }
		public VirtualAttribute(AllowedInheritanceOption option)
		{
			Option = option;
		}
	}
}