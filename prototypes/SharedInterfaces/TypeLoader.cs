using System;

namespace SharedInterfaces
{
    public class TypeLoader : MarshalByRefObject
    {
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
            return
                (TypeLoader)
                    domain.CreateInstance(TypeLoaderAssemblyName,
                        typeof (TypeLoader).FullName).Unwrap();
        }
    }
}