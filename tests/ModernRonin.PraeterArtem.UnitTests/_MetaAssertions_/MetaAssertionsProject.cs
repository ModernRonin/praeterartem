using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit.Sdk;

namespace ModernRonin.PraeterArtem.UnitTests._MetaAssertions_
{
	public sealed class MetaAssertionsProject 
	{
		[NotNull]
		public Assembly CodeAssembly { get; private set; }

		[NotNull]
		public Assembly TestAssembly { get; private set; }

		[NotNull] private readonly List<IMetaAssertionsType> mTypes = new List<IMetaAssertionsType>();

		[NotNull]
		public IEnumerable<IMetaAssertionsType> Types {  get { return mTypes; } }

		#region ' Ugly nested class '
		sealed class CodeAssemblyNotFound : IMetaAssertionsType
		{
			readonly string mReplace;

			public CodeAssemblyNotFound([NotNull] string replace)
			{
				mReplace = replace;
			}

			public void Check()
			{
				throw new AssertException(
					"No '" + mReplace + "' library is found. " +
					"Perhaps it's never accessed from code explicitly");
			}

			public override string ToString()
			{
				return "Not found: " + mReplace;
			}
		}
		#endregion


		MetaAssertionsProject([NotNull] Assembly testAssembly)
		{
			TestAssembly = testAssembly;

			var codeAssembly = GetCodeAssemblyForTestAssembly(testAssembly);
			if (codeAssembly == null) return;
			CodeAssembly = codeAssembly;

			mTypes.AddRange(from t in CodeAssembly.GetTypes()
				select new MetaAssertionsType(t, TestAssembly));

			mTypes.AddRange(from t in TestAssembly.GetTypes()
				select new MetaAssertionsType(t));

			mTypes.RemoveAll(t => ((MetaAssertionsType) t).IsCompilerGenerated);
		}

		[CanBeNull]
		Assembly GetCodeAssemblyForTestAssembly([NotNull] Assembly testAssembly)
		{
			var replace = new AssemblyName(testAssembly.FullName).Name.Replace(".UnitTests", "");
			var assemblyName = testAssembly.GetReferencedAssemblies().SingleOrDefault(r => r.Name == replace);
			if (assemblyName == null)
			{
				mTypes.Add(new CodeAssemblyNotFound(replace));
				return null;
			}

			return Assembly.Load(assemblyName);
		}

		/// <typeparam name="TTest">Any type from TestAssembly</typeparam>
		[NotNull]
		public static MetaAssertionsProject FromSampleClass<TTest>()
		{
			return new MetaAssertionsProject(typeof (TTest).Assembly);
		}
	}
}