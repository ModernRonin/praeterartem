using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using ModernRonin.PraeterArtem.Reflection;
using ModernRonin.PraeterArtem.UnitTests.Properties;
using SharedInterfacesForTestingRemoting;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Reflection
{
    public sealed class AppDomainExtensionsTests
    {
        const string NameOfDynamicAssembly = "DynamicAssembly";
        public AppDomainExtensionsTests()
        {
            if (File.Exists(FileNameOfDynamicAssembly))
                File.Delete(FileNameOfDynamicAssembly);
        }
        string FileNameOfDynamicAssembly
        {
            get { return string.Format("{0}.dll", NameOfDynamicAssembly); }
        }
        [Fact(
            Skip =
                "Weird things seem to happen when doing this inside a test runner - while from a console program it works just fine. For the time being, I give up on writing unit-tests for this."
            )]
        [Category("DoesNotRunUnderNCrunch")]
        public void CreateTypeInDomain()
        {
            CreateAssembly();

            var secondaryAppDomain = AppDomain.CreateDomain("alpha");
            var dynamicallyCreated =
                secondaryAppDomain.CreateTypeInDomain<IRemoteType>(
                                                                   FileNameOfDynamicAssembly,
                    "GeneratedAssembly.Implementation");
        }
        void CreateAssembly()
        {
            var parameters = new CompilerParameters
                             {
                                 GenerateExecutable =
                                     false,
                                 GenerateInMemory =
                                     false,
                                 OutputAssembly =
                                     NameOfDynamicAssembly
                             };
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add(
                                                "SharedInterfacesForTestingRemoting.dll");
            var compiler = CodeDomProvider.CreateProvider("CSharp");
            var results = compiler.CompileAssemblyFromSource(parameters,
                Resources.CodeForGeneratedAssembly);
            if (0 < results.Errors.Count)
            {
                results.Errors.Cast<CompilerError>().UseIn(Console.WriteLine);
                Assert.True(false);
            }
        }
        [Fact(
            Skip =
                "Weird things seem to happen when doing this inside a test runner - while from a console program it works just fine. For the time being, I give up on writing unit-tests for this."
            )]
        public void Execute_Executes_Action_In_Other_AppDomain()
        {
            var secondaryDomain = AppDomain.CreateDomain("Bravo");
            AppDomain.CurrentDomain.FriendlyName.Should().NotBe("Bravo");
            secondaryDomain.FriendlyName.Should().Be("Bravo");

            secondaryDomain.Execute(
                                    () =>
                                        AppDomain.CurrentDomain.FriendlyName
                                                 .Should().Be("Bravo"));
        }
    }
}