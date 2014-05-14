using System;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    [UsedImplicitly]
    sealed class RemoteDomainExecutor : MarshalByRefObject
    {
        public static void ExecuteIn(AppDomain domain, Action action)
        {
            var executor =
                TypeInAppDomainCreator.CreateTypeIn<RemoteDomainExecutor>(
                                                                          domain);
            executor.Execute(action);
        }
        /// <summary>
        ///     This MUST be an instance method, otherwise it will be called in the default AppDomain.
        /// </summary>
        void Execute(Action action)
        {
            action();
        }
    }
}