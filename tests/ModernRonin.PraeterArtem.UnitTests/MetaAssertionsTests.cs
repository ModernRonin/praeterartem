using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.UnitTests._MetaAssertions_;
using NUnit.Framework;

namespace ModernRonin.PraeterArtem.UnitTests
{
	[TestFixture]
	public sealed class MetaAssertionsTests
	{
		[NotNull]
		static IEnumerable<IMetaAssertionsType> GetTypesToTest()
		{
			return MetaAssertionsProject.FromSampleClass<MetaAssertionsTests>().Types;
		}

		[Test]
		[TestCaseSource("GetTypesToTest")]
		public void Check([NotNull] IMetaAssertionsType type)
		{
			Console.WriteLine(type.ToString());
			type.Check();
		}
	}
}