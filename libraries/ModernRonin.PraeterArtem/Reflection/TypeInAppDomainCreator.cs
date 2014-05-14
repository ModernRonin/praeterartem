using System;

namespace ModernRonin.PraeterArtem.Reflection
{
    static class TypeInAppDomainCreator
    {
        static string GetAssemblyName<TSelf>()
        {
            return typeof(TSelf).Assembly.GetName().Name;
        }
        public static T CreateTypeIn<T>(AppDomain domain)
        {
            return
                (T)
                    domain.CreateInstance(GetAssemblyName<T>(),
                        typeof (T).FullName).Unwrap();
        }
    }
}