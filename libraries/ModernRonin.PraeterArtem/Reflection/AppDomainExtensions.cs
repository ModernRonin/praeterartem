using System;

namespace ModernRonin.PraeterArtem.Reflection
{
    public static class AppDomainExtensions
    {
        public static T CreateTypeInDomain<T>(this AppDomain domain,
                                              string assemblyFilePath,
                                              string concreteTypeName)
        {
            return TypeLoader.CreateTypeInDomain<T>(domain, assemblyFilePath,
                concreteTypeName);
        }
    }
}