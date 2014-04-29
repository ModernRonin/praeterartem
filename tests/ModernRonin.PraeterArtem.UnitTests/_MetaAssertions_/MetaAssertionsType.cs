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
        public TypeSource Source { get; private set; }

	    [NotNull]
	    public Type TargetType { get; private set; }

	    [NotNull]
	    public Type TestType { get { return GetUnitTest(TargetType, mTestAssembly); } }

        public bool IsVirtual { get { return TargetType.HasAttribute<VirtualAttribute>(); } }
        public bool IsDataTransferObject { get { return TargetType.HasAttribute<DataTransferObjectAttribute>(); } }
        public bool IsUnitTestsNeeded
        {
            get
            {
                return !TargetType
                    .GetCustomAttributes(false)
                    .Any(x => AttributesThatMeanNoUnitTestsNeeded.Contains(x.GetType()));
            }
        }
        public bool HasPublicMethods { get { return TargetType.HasPublicMethods(); } }
        public bool IsCompilerGenerated
        {
            get
            {
                return TargetType.FullName.Contains("JetBrains.Profiler.Core.Instrumentation")
                    || TargetType.HasAttribute<CompilerGeneratedAttribute>();
            }
        }
        public bool HasSpecialNamespace
        {
            get
            {
                return TargetType.Namespace != null &&
                       TargetType.Namespace.Split('.').Any(part =>
                          part.StartsWith("_", StringComparison.Ordinal) && 
						  part.EndsWith("_", StringComparison.Ordinal));
            }
        }

        public MetaAssertionsType([NotNull] Type testType)
        {
	        Source = TypeSource.TestAssembly;
	        TargetType = testType;
        }

	    public MetaAssertionsType([NotNull] Type codeType, [NotNull] Assembly testAssembly)
	    {
		    mTestAssembly = testAssembly;
		    Source = TypeSource.CodeAssembly;
		    TargetType = codeType;
	    }

	    public override string ToString()
        {
            return TargetType.ToString();
        }

        public void Check()
        {
            Debug.WriteLine(TargetType.ToString());

            if (Source == TypeSource.CodeAssembly)
                if (IsUnitTestsNeeded)
					if (TargetType.Namespace != null)
					if (TargetType.Namespace != "JetBrains.Annotations")
					if (!TargetType.Namespace.EndsWith("Migrations", StringComparison.Ordinal))
					if (!TargetType.IsEnum && !TargetType.IsInterface)
                        if (TargetType.IsNestedPublic || !TargetType.IsNested)
								if (TestType == null)
									throw new AssertException("Every code type should have test " +
										"\r\nor should be marked with one of the attributes " +
										string.Join("\r\n", AttributesThatMeanNoUnitTestsNeeded
										.Select(x => "[" + x.Name.Replace("Attribute", "") + "]")));

            if (Source == TypeSource.TestAssembly)
                if (TargetType.IsClass && !TargetType.IsNested && !HasSpecialNamespace)
                {
                    TargetType.IsPublic.Should().BeTrue("Every test type should be public");

                    TargetType.Name.Should().EndWith("Tests",
                        "Every test type should have name ending with 'Tests'");
                }

            if (TargetType.IsClass && !TargetType.IsAbstract)
                TargetType.IsSealed.Should().Be(!IsVirtual,
                    "Every type should be either sealed, or [Virtual], or abstract");

            if (IsDataTransferObject)
                HasPublicMethods.Should().BeFalse(
					"Every [DataTransferObject] type should have no public methods");
        }

        static HashSet<Type> sAttributesThatMeanNoUnitTestsNeeded;

	    [NotNull]
	    public static HashSet<Type> AttributesThatMeanNoUnitTestsNeeded
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
        static HashSet<Type> sAttributesThatMeanIntegrationTestNeeded;

	    [NotNull]
	    public static HashSet<Type> AttributesThatMeanIntegrationTestNeeded
        {
            get
            {
                return sAttributesThatMeanIntegrationTestNeeded ??
                      (sAttributesThatMeanIntegrationTestNeeded =
                       new HashSet<Type>(
                           typeof(MeansIntegrationTestNeededAttribute).Assembly.GetTypes()
                           .Where(x => x.HasAttribute<MeansIntegrationTestNeededAttribute>())
                           .ToList()));
            }
        }

	    [NotNull]
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