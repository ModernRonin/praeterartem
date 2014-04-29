using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.UnitTests._MetaAssertions_;
using Xunit.Extensions;

namespace ModernRonin.PraeterArtem.UnitTests
{
	public sealed class MetaAssertionsTests
	{
		[NotNull]
		public static IEnumerable<IMetaAssertionsType[]> TypesToTest
		{
			get
			{
				return MetaAssertionsProject
					.FromSampleClass<MetaAssertionsTests>()
					.Types.Select(x => new[] {x});
			}
		}

		[Theory]
		[PropertyData("TypesToTest")]
		public void Check([NotNull] IMetaAssertionsType type)
		{
			Console.WriteLine(type.ToString());
			type.Check();
		}
	}
}