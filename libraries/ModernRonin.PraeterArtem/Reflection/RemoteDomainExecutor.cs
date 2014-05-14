using System;

namespace ModernRonin.PraeterArtem.Reflection
{
    sealed class RemoteDomainExecutor : MarshalByRefObject
    {
        static string MyAssemblyName
        {
            get
            {
                return typeof (RemoteDomainExecutor).Assembly.GetName().Name;
            }
        }
        public static void ExecuteIn(AppDomain domain, Action action)
        {
            var executor =
                (RemoteDomainExecutor)
                    domain.CreateInstance(MyAssemblyName,
                        typeof (RemoteDomainExecutor).FullName).Unwrap();
            executor.Execute(action);
        }
        void Execute(Action action)
        {
            action();
        }
    }
}