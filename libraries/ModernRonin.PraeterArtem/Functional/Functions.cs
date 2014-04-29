using System;

namespace ModernRonin.PraeterArtem.Functional
{
    /// <summary>
    /// Methods supporting a functional coding style related to Func{T}.
    /// </summary>
    public static class Functions
    {
		// Ilya: If only this method name was shorter than Functions.Identity<T>()!
		// in C# 6 you will be able to add the class name to using but it is long :(
        /// <summary>Returns a Func{T} which always returns its argument</summary>
        public static Func<T, T> Identity<T>()
        {
            return x => x;
        }
    }
}