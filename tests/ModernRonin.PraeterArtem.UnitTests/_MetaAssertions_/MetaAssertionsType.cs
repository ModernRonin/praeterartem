using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using JetBrains.Annotations;
using ModernRonin.PraeterArtem.Annotations;
using ModernRonin.PraeterArtem.Reflection;
using Xunit.Sdk;

namespace ModernRonin.PraeterArtem.UnitTests._MetaAssertions_
{
    public sealed class MetaAssertionsType : IMetaAssertionsType
    {
        readonly Assembly mTestAssembly;
	    private readonly TypeSource mSource;

	    [NotNull] private readonly Type mTargetType;

	    [CanBeNull]
		 Type TestType { get { return GetUnitTest(mTargetType, mTestAssembly); } }

         bool IsVirtual { get { return mTargetType.HasAttribute<VirtualAttribute>(); } }
         bool IsDataTransferObject { get { return mTargetType.HasAttribute<DataTransferObjectAttribute>(); } }
         bool IsUnitTestsNeeded
        {
            get
            {
                if (!mTargetType.IsVisible)
                    return false;
                return !mTargetType
                    .GetCustomAttributes(false)
                    .Any(x => AttributesThatMeanNoUnitTestsNeeded.Contains(x.GetType()));
            }
        }
         bool HasPublicMethods { get { return mTargetType.HasPublicMethods(); } }
        public bool IsCompilerGenerated
        {
            get
            {
                return mTargetType.FullName.Contains("JetBrains.Profiler.Core.Instrumentation")
                    || mTargetType.HasAttribute<CompilerGeneratedAttribute>();
            }
        }
         bool HasSpecialNamespace
        {
            get
            {
                return mTargetType.Namespace != null &&
                       mTargetType.Namespace.Split('.').Any(part =>
                          part.StartsWith("_", StringComparison.Ordinal) && 
						  part.EndsWith("_", StringComparison.Ordinal));
            }
        }

        public MetaAssertionsType([NotNull] Type testType)
        {
	        mSource = TypeSource.TestAssembly;
	        mTargetType = testType;
        }

	    public MetaAssertionsType([NotNull] Type codeType, [NotNull] Assembly testAssembly)
	    {
		    mTestAssembly = testAssembly;
		    mSource = TypeSource.CodeAssembly;
		    mTargetType = codeType;
	    }

	    public override string ToString()
        {
            return mTargetType.ToString();
        }

        public void Check()
        {
            Debug.WriteLine(mTargetType.ToString());

            if (mSource == TypeSource.CodeAssembly)
                if (IsUnitTestsNeeded)
					if (mTargetType.Namespace != null)
					if (mTargetType.Namespace != "JetBrains.Annotations")
					if (!mTargetType.Namespace.EndsWith("Migrations", StringComparison.Ordinal))
					if (!mTargetType.IsEnum && !mTargetType.IsInterface)
                        if (mTargetType.IsNestedPublic || !mTargetType.IsNested)
								if (TestType == null)
									throw new AssertException("Every code type should have test " +
										"\r\nor should be marked with one of the attributes " +
										string.Join("\r\n", AttributesThatMeanNoUnitTestsNeeded
										.Select(x => "[" + x.Name.Replace("Attribute", "") + "]")));

            if (mSource == TypeSource.TestAssembly)
                if (mTargetType.IsClass && !mTargetType.IsNested && !HasSpecialNamespace)
                {
                    mTargetType.IsPublic.Should().BeTrue("Every test type should be public");

                    mTargetType.Name.Should().EndWith("Tests",
                        "Every test type should have name ending with 'Tests'");
                }

            if (mTargetType.IsClass && !mTargetType.IsAbstract)
                mTargetType.IsSealed.Should().Be(!IsVirtual,
                    "Every type should be either sealed, or [Virtual], or abstract");

            if (IsDataTransferObject)
                HasPublicMethods.Should().BeFalse(
					"Every [DataTransferObject] type should have no public methods");
        }

        static HashSet<Type> sAttributesThatMeanNoUnitTestsNeeded;

	    [NotNull]
	    static HashSet<Type> AttributesThatMeanNoUnitTestsNeeded
        {
            get
            {
                return sAttributesThatMeanNoUnitTestsNeeded ??
                      (sAttributesThatMeanNoUnitTestsNeeded =
                       new HashSet<Type>(
                           typeof(MeansNoUnitTestsNeededAttribute).Assembly.GetTypes()
                           .Where(x => x.HasAttribute<MeansNoUnitTestsNeededAttribute>())
						   .Concat(new[] { typeof(GeneratedCodeAttribute) })
                           .ToList()));
            }
        }
      
	    [CanBeNull]
	    private static Type GetUnitTest([NotNull] Type type, [NotNull] Assembly unitTestsAssembly)
	    {
		    var ns = type.Namespace;
		    if (ns == null) return null;
		    var root = SubstringOnLeftOfFirst(type.Assembly.FullName, ",");
		    var replace = ns.Replace(root, string.Format(
			    CultureInfo.InvariantCulture, "{0}.UnitTests", root));
		    var className = SubstringOnLeftOfFirst(type.Name, "`");
		    var name = replace + "." + className + "Tests";
		    Debug.WriteLine("Looking for: " + name);
		    return unitTestsAssembly.GetType(name);
	    }

	    [NotNull]
	    private static string SubstringOnLeftOfFirst([NotNull] string str, [NotNull] string sub)
	    {
		    var idx = str.IndexOf(sub, StringComparison.Ordinal);
		    return idx != -1 ? str.Substring(0, idx) : str;
	    }
    }
}