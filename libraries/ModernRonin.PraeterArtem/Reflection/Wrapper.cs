using System;

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
}