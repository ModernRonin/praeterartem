using System;
using System.Diagnostics.CodeAnalysis;

namespace JetBrains.Annotations
{
	/// <summary>
	/// When applied to target attribute, specifies a requirement for any type which is marked with 
	/// target attribute to implement or inherit specific type or types
	/// </summary>
	/// <example>
	/// <code>
	/// [BaseTypeRequired(typeof(IComponent)] // Specify requirement
	/// public class ComponentAttribute : Attribute 
	/// {}
	/// 
	/// [Component] // ComponentAttribute requires implementing IComponent interface
	/// public class MyComponent : IComponent
	/// {}
	/// </code>
	/// </example>
	[SuppressMessage("Microsoft.Design", 
		"CA1019:DefineAccessorsForAttributeArguments",
		Justification = "It is JetBrains code")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[BaseTypeRequired(typeof (Attribute))]
	public sealed class BaseTypeRequiredAttribute : Attribute
	{
		/// <summary>
		/// Initializes new instance of BaseTypeRequiredAttribute
		/// </summary>
		/// <param name="baseType">Specifies which types are required</param>
		public BaseTypeRequiredAttribute([NotNull] Type baseType)
		{
			BaseTypes = new[] {baseType};
		}

		/// <summary>
		/// Gets enumerations of specified base types
		/// </summary>
		[NotNull][SuppressMessage("Microsoft.Performance",
			"CA1819:PropertiesShouldNotReturnArrays",
			Justification = "It is JetBrains code")]
		public Type[] BaseTypes { get; private set; }
	}
}