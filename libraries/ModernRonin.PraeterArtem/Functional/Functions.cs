using System;

namespace ModernRonin.PraeterArtem.Functional
{
    /// <summary>
    /// Methods supporting a functional coding style related to Func{T}.
    /// </summary>
    public static class Functions
    {
        /// <summary>Returns a Func{T} which always returns its argument</summary>
        public static Func<T, T> Identity<T>()
        {
            return x => x;
        }
    }
}