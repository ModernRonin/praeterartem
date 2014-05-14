using System;
using System.IO;
using SharedInterfaces;

namespace DomainLoader
{
    class Program
    {
        static string TypeLoaderAssemblyName
        {
            get
            {
                return "SharedInterfaces";
            }
        }
        void Run()
        {
            Log("Starting");
            Log("My own AppDomain: {0}", AppDomain.CurrentDomain.Id);
            Log("Creating AppDomain");
            var loadedDomain = AppDomain.CreateDomain("Dynamically Loaded");
            Log("Loaded Domain: {0}", loadedDomain.Id);
            // TODO: list loaded domains
            Log("Creating TypeLoader in Loaded Domain");
            var typeLoader =
                (TypeLoader)
                    loadedDomain.CreateInstance(TypeLoaderAssemblyName,
                        typeof (TypeLoader).FullName).Unwrap();
            Log("TypeLoader's AppDomain: {0}", typeLoader.AppDomainIdentifier);
            Log("Using TypeLoader to get an instance of concrete type as an interface");
            var remoteType =
                typeLoader.Load("../../../SomeLibrary/bin/debug/SomeLibrary.dll", "SomeLibrary.AppDomainBoundaryCrosser");
            Log("RemoteType's AppDomain: {0}", remoteType.AppDomainIdentifier);

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