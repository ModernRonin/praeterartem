using System;
using System.Linq;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    [Serializable]
    sealed class Wrapper<TArgument, TResult>
    {
        readonly TArgument mArgument;
        readonly Func<TArgument, TResult> mFunction;
        public Wrapper(TArgument argument, Func<TArgument, TResult> function)
        {
            mArgument = argument;
            mFunction = function;
        }
        public TResult Execute()
        {
            return mFunction(mArgument);
        }
    }

    [UsedImplicitly]
    sealed class RemoteDomainExecutor : MarshalByRefObject
    {
        public static void ExecuteIn(AppDomain domain, Action action)
        {
            var executor =
                CreateExecutor(domain);
            executor.Execute(action);
        }
        static RemoteDomainExecutor CreateExecutor(AppDomain domain)
        {
            return TypeInAppDomainCreator.CreateTypeIn<RemoteDomainExecutor>(
                                                                             domain);
        }
        public static TResult ExecuteIn<TArgument, TResult>(AppDomain domain, TArgument argument,
                                                            Func
                                                                <TArgument,
                                                                TResult>
                                                                function)
        {
            var executor =
                CreateExecutor(domain);
            var wrapper = new Wrapper<TArgument, TResult>(argument, function);
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