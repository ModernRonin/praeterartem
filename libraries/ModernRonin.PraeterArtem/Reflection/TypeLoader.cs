using System;
using System.Collections.Concurrent;

namespace ModernRonin.PraeterArtem.Reflection
{
    internal sealed class TypeLoader : MarshalByRefObject
    {
        static readonly ConcurrentDictionary<AppDomain, TypeLoader>
            sAppDomainsToInstances =
                new ConcurrentDictionary<AppDomain, TypeLoader>();
        static string TypeLoaderAssemblyName
        {
            get { return typeof (TypeLoader).Assembly.GetName().Name; }
        }
        public T Load<T>(string assemblyFilePath, string concreteTypeName)
        {
            var result =
                Activator.CreateInstanceFrom(assemblyFilePath,
                    concreteTypeName).Unwrap();
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
        public static T CreateTypeInDomain<T>(AppDomain domain,
                                              string assemblyFilePath,
                                              string concreteTypeName)
        {
            var typeLoader = Create(domain);
            return typeLoader.Load<T>(assemblyFilePath, concreteTypeName);
        }
    }
}