using System;

namespace SharedInterfaces
{
    public class TypeLoader : AnAppDomainIdentifiable
    {
        public T Load<T>(string assemblyName, string concreteTypeName)
        {
            var result =
                Activator.CreateInstanceFrom(assemblyName, concreteTypeName)
                         .Unwrap();
            return (T) result;
        }
    }
}