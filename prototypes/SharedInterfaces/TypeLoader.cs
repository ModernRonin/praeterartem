using System;

namespace SharedInterfaces
{
    public class TypeLoader : AnAppDomainIdentifiable
    {
        public IAppDomainIdentifiable Load(string assemblyName,
                                           string concreteTypeName)
        {
            return
                (IAppDomainIdentifiable)
                    Activator.CreateInstanceFrom(assemblyName,
                        concreteTypeName).Unwrap();
        }
    }
}