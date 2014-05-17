using System;

namespace ModernRonin.PraeterArtem.Reflection
{
    [Serializable]
    sealed class FunctionWrapper<TArgument, TResult>
    {
        readonly TArgument mArgument;
        readonly Func<TArgument, TResult> mFunction;
        public FunctionWrapper(TArgument argument,
                               Func<TArgument, TResult> function)
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