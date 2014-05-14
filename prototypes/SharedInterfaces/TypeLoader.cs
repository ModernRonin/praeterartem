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

    public class AppDomainExecutor : MarshalByRefObject
    {
        void DoExecute(Action action)
        {
            action();
        }
        void DoExecute<T>(T parameter, Action<T> action)
        {
            action(parameter);
        }
    }
}