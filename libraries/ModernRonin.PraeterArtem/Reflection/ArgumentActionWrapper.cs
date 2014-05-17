using System;

namespace ModernRonin.PraeterArtem.Reflection
{
    [Serializable]
    sealed class ArgumentActionWrapper<T>
    {
        readonly Action<T> mAction;
        readonly T mArgument;
        public ArgumentActionWrapper(T argument, Action<T> action)
        {
            mArgument = argument;
            mAction = action;
        }
        public void Execute()
        {
            mAction(mArgument);
        }
    }
}