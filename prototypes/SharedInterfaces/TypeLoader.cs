using System;
using System.Collections.Concurrent;

namespace SharedInterfaces
{
    public class TypeLoader : MarshalByRefObject
    {
        static readonly ConcurrentDictionary<AppDomain, TypeLoader>
            sAppDomainsToInstances =
                new ConcurrentDictionary<AppDomain, TypeLoader>();
        static string TypeLoaderAssemblyName
        {
            get { return typeof (TypeLoader).Assembly.GetName().Name; }
        }
        public int AppDomainIdentifier
        {
            get { return AppDomain.CurrentDomain.Id; }
        }
        public T Load<T>(string assemblyName, string concreteTypeName)
        {
            var result =
                Activator.CreateInstanceFrom(assemblyName, concreteTypeName)
                         .Unwrap();
            return (T) result;
        }
        public static TypeLoader Create(AppDomain domain)
        {
            return sAppDomainsToInstances.GetOrAdd(domain, DoCreate);
        }
        static TypeLoader DoCreate(AppDomain domain)
        {
            var result =
                (TypeLoader)
                    domain.CreateInstance(TypeLoaderAssemblyName,
                        typeof (TypeLoader).FullName).Unwrap();
            domain.DomainUnload += OnDomainUnloading;
            return result;
        }
        static void OnDomainUnloading(object sender, EventArgs e)
        {
            Remove((AppDomain) sender);
        }
        static void Remove(AppDomain domain)
        {
            domain.DomainUnload -= OnDomainUnloading;
            TypeLoader ignore;
            sAppDomainsToInstances.TryRemove(domain, out ignore);
        }
    }
}