using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    [UsedImplicitly]
    sealed class TypeLoader : MarshalByRefObject
    {
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
            var result =
                TypeInAppDomainCreator.CreateTypeIn<TypeLoader>(domain);
            return result;
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