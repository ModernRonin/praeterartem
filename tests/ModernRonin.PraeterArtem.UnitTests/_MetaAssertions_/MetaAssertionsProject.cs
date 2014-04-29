using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace ModernRonin.PraeterArtem.UnitTests._MetaAssertions_
{
	public sealed class MetaAssertionsProject 
	{
		public Assembly CodeAssembly { get; private set; }
		public Assembly TestAssembly { get; private set; }
		private readonly List<IMetaAssertionsType> mTypes = new List<IMetaAssertionsType>();

		public IEnumerable<IMetaAssertionsType> Types {  get { return mTypes; } }

		#region ' Ugly nested class '
		sealed class CodeAssemblyNotFound : IMetaAssertionsType
		{
			readonly string mReplace;

			public CodeAssemblyNotFound(string replace)
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


		MetaAssertionsProject(Assembly testAssembly)
		{
			TestAssembly = testAssembly;

			var codeAssembly = GetCodeAssemblyForTestAssembly(testAssembly);
			if (codeAssembly == null) return;
			CodeAssembly = codeAssembly;

			mTypes.AddRange(from t in CodeAssembly.GetTypes()
					 select new MetaAssertionsType(t, TestAssembly));

			mTypes.AddRange(from t in TestAssembly.GetTypes()
					 select new MetaAssertionsType(t));

			mTypes.RemoveAll(t => ((MetaAssertionsType)t).IsCompilerGenerated);
		}

		Assembly GetCodeAssemblyForTestAssembly(Assembly testAssembly)
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
		public static MetaAssertionsProject FromSampleClass<TTest>()
		{
			return new MetaAssertionsProject(typeof(TTest).Assembly);
		}
	}
}