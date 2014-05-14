using System;
using System.Collections.Generic;
using System.Linq;
using ModernRonin.PraeterArtem.Functional;
using mscoree;
using SharedInterfaces;

namespace DomainLoader
{
    [Serializable]
    class Program
    {
        static readonly string[] sIgnorableAssemblyNamePatterns =
        {
            "Microsoft",
            "System",
            "mscorlib",
            "vshost32"
        };
        static string TypeLoaderAssemblyName
        {
            get { return typeof (TypeLoader).Assembly.GetName().Name; }
        }
        void Run()
        {
            Log("Starting");
            Log("My own AppDomain: {0}", AppDomain.CurrentDomain.Id);
            ListLoadedAssembliesInCurrentDomain();
            Log("Creating AppDomain");
            var loadedDomain = AppDomain.CreateDomain("Dynamically Loaded");
            Log("Loaded Domain: {0}", loadedDomain.Id);
            ListLoadedDomains();

            Log(
                "Using Extension Method to get an instance of concrete type as an interface in the Loaded Domain");
            var remoteType =
                loadedDomain.CreateTypeInDomain<IRemoteType>(
                                                             "../../../SomeLibrary/bin/debug/SomeLibrary.dll",
                    "SomeLibrary.SomeImplementation");
            Log("RemoteType's AppDomain: {0}", remoteType.AppDomainIdentifier);
            ListLoadedAssembliesInCurrentDomain();

            remoteType.Execute(ListLoadedAssembliesInCurrentDomain);
            
            Log("Unloading Loaded Domain");
            AppDomain.Unload(loadedDomain);
            ListLoadedDomains();
        }
        void ListLoadedDomains()
        {
            Log("Domains loaded:");
            var appDomains = GetLoadedAppDomains();

            appDomains.Select(d => d.Id).UseIn(id => Log("\t#{0}", id));
            LogEndOfList();
        }
        static IEnumerable<AppDomain> GetLoadedAppDomains()
        {
            var appDomains = new List<AppDomain>();
            IntPtr handle;
            var runtimeHost = new CorRuntimeHost();
            runtimeHost.EnumDomains(out handle);
            var doContinue = true;
            while (doContinue)
            {
                object domain;
                runtimeHost.NextDomain(handle, out domain);
                if (domain == null)
                    doContinue = false;
                else
                    appDomains.Add((AppDomain) domain);
            }
            runtimeHost.CloseEnum(handle);
            return appDomains;
        }
        void ListLoadedAssembliesInCurrentDomain()
        {
            var domain = AppDomain.CurrentDomain;
            Log("AppDomain #{0}'s loaded assemblies:", domain.Id);
            domain.GetAssemblies()
                  .Select(a => a.GetName().Name)
                  .Where(n => !IsFilteredNamespace(n))
                  .OrderBy(Functions.Identity<string>())
                  .UseIn(n => Log("\t{0}", n));
            LogEndOfList();
        }
        static bool IsFilteredNamespace(string name)
        {
            return sIgnorableAssemblyNamePatterns.Any(name.StartsWith);
        }
        void LogEndOfList()
        {
            Log("-------------------------------------------------------");
        }
        static void Main()
        {
            new Program().Run();
            Console.WriteLine("Press Enter to end");
            Console.ReadLine();
        }
        void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}