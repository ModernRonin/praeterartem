using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModernRonin.PraeterArtem.Functional;
using ModernRonin.PraeterArtem.Reflection;
using mscoree;
using SharedInterfaces;

namespace DomainLoader
{
    class SomeType : MarshalByRefObject
    {
        public SomeType(string name)
        {
            Console.WriteLine("{0} is being constructed in AppDomain #{1}",
                typeof (SomeType).PrettyName(), AppDomain.CurrentDomain.Id);
            Name = name;
        }
        public string Name { get; private set; }
    }

    interface IMessageList : IEnumerable<string>
    {
        void Add(string msg);
    }

    [Serializable]
    class MessageList : MarshalByRefObject, IMessageList
    {
        readonly List<string> mMessages = new List<string>();
        public IEnumerator<string> GetEnumerator()
        {
            return mMessages.GetEnumerator();
        }
        public void Add(string msg)
        {
            mMessages.Add(msg);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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
        readonly IMessageList mMessageList = new MessageList {"Lima"};
        readonly StringWriter mStandardError = new StringWriter();
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

            LogEndOfList();
            Log("Executing nullary action lambda in Loaded Domain");
            loadedDomain.Execute(
                                 () =>
                                     Log("Lambda's AppDomain: {0}",
                                         AppDomain.CurrentDomain.Id));

            LogEndOfList();
            Log("Executing unary function lambda in Loaded Domain");
            var result = loadedDomain.Execute("Zulu",
                name => new SomeType(name).Name);
            Log("Result: {0}", result);

            Log("Executing unary action lamba in Loaded Domain");
            loadedDomain.Execute("Quebec", UseString);
            LogEndOfList();

            LogEndOfList();
            Log("Sending interface as parameter to Loaded Domain");
            loadedDomain.Execute(() => UseInterface(mMessageList));
            Log("Result: {0}", string.Join(", ", mMessageList));
            LogEndOfList();

            Console.SetError(mStandardError);
            Console.Error.Write("Oscar");
            Log("Using redirected console in Loaded Domain");
            remoteType.WriteError();
            Log("Result: My StdErr: {0}, remoteType.Error: {1}",
                mStandardError.ToString(), remoteType.Error);

            LogEndOfList();
            Log("Unloading Loaded Domain");
            AppDomain.Unload(loadedDomain);
            ListLoadedDomains();
        }
        static void UseString(string text)
        {
            Log(
                "UseString() being called in AppDomain #{0} with parameter {1}",
                AppDomain.CurrentDomain.Id, text);
        }
        static void UseInterface(IMessageList list)
        {
            Log("UseInterface() being called in AppDomain #{0}",
                AppDomain.CurrentDomain.Id);
            if (null == list)
                throw new ArgumentNullException("list");
            list.Add("Mike");
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
        static void LogEndOfList()
        {
            Log("-------------------------------------------------------");
        }
        static void Main()
        {
            new Program().Run();
            Console.WriteLine("Press Enter to end");
            Console.ReadLine();
        }
        static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}