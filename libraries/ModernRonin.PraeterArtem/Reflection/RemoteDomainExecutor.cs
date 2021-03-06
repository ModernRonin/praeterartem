using System;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    [UsedImplicitly]
    sealed class RemoteDomainExecutor : MarshalByRefObject
    {
        public static void ExecuteIn(AppDomain domain, Action action)
        {
            var executor = CreateExecutor(domain);
            executor.Execute(action);
        }
        public static void ExecuteIn<T>(AppDomain domain, T argument,
                                        Action<T> action)
        {
            var executor = CreateExecutor(domain);
            var wrapper = new ArgumentActionWrapper<T>(argument, action);
            executor.Execute(wrapper.Execute);
        }
        static RemoteDomainExecutor CreateExecutor(AppDomain domain)
        {
            return
                TypeInAppDomainCreator.CreateTypeIn<RemoteDomainExecutor>(
                                                                          domain);
        }
        public static TResult ExecuteIn<TArgument, TResult>(AppDomain domain,
                                                            TArgument
                                                                argument,
                                                            Func
                                                                <TArgument,
                                                                TResult>
                                                                function)
        {
            var executor = CreateExecutor(domain);
            var wrapper = new FunctionWrapper<TArgument, TResult>(argument,
                function);
            return executor.Execute<TResult>(wrapper.Execute);
        }
        /// <summary>
        ///     This MUST be an instance method, otherwise it will be called in the default AppDomain.
        /// </summary>
        void Execute(Action action)
        {
            action();
        }
        T Execute<T>(Func<T> function)
        {
            return function();
        }
    }
}