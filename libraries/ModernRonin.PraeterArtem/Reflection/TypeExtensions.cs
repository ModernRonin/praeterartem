using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
	/// <summary>
	/// Provides extension methods for the <see cref="Type"/> class.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>Indicates whether one or more attributes of the specified type or of its derived types is applied to this member.</summary>
		/// <typeparam name="TAttribute">The attribute to look for.</typeparam>
		/// <returns>true if a custom attribute of type attributeType is applied to element; otherwise, false.</returns>
		public static bool HasAttribute<TAttribute>([NotNull] this Type type)
			where TAttribute : Attribute
		{
			return Attribute.IsDefined(type, typeof (TAttribute));
		}

		/// <summary> Indicates whether there are public methods declared on the type (and not base type)</summary>
		/// <returns>true if any method is located in the specified type; otherwise, false</returns>
		public static bool HasPublicMethods([NotNull] this Type type)
		{
			return type
				.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
				.Where(m => !m.IsSpecialName)
				.Any(m => m.DeclaringType == type);
		}
	}
}