using System;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Functional
{
    /// <summary>
    ///     Extensions method on integers supporting a functional coding style
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        ///     Executes <paramref name="action" /> <paramref name="count" /> times.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        public static void TimesExecute(this int count, [NotNull] Action action)
        {
	        if (action == null) throw new ArgumentNullException("action");
	        for (var i = 0; i < count; ++i)
                action();
        }

	    /// <summary>
        ///     Executes <paramref name="action" /> for all integers in the
        ///     interval [0; <paramref name="count" />[.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        public static void TimesExecute(this int count, [NotNull] Action<int> action)
	    {
		    if (action == null) throw new ArgumentNullException("action");
		    for (var i = 0; i < count; ++i)
                action(i);
	    }
    }
}