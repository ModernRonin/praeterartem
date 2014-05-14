using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    [UsedImplicitly]
    sealed class TypeLoader : MarshalByRefObject
    {
        static readonly ConcurrentDictionary<AppDomain, TypeLoader>
            sAppDomainsToInstances =
                new ConcurrentDictionary<AppDomain, TypeLoader>();
        /// <summary>
        ///     This MUST be an instance method, otherwise it will be called in the default AppDomain.
        /// </summary>
        T Load<T>(string assemblyFilePath, string concreteTypeName)
        {
            var result =
                Activator.CreateInstanceFrom(assemblyFilePath,
                    concreteTypeName).Unwrap();
            return (T) result;
        }
        static TypeLoader Create(AppDomain domain)
        {
            return sAppDomainsToInstances.GetOrAdd(domain, DoCreate);
        }
        static TypeLoader DoCreate(AppDomain domain)
        {
            var result =
                TypeInAppDomainCreator.CreateTypeIn<TypeLoader>(domain);
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