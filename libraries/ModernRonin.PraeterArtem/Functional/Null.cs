using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Functional
{
    public static class Null
    {
	    [NotNull]
	    public static IEnumerable<T> Enumerable<T>()
	    {
		    return System.Linq.Enumerable.Empty<T>();
	    }

	    [NotNull]
	    public static Action Action()
	    {
		    return () => { };
	    }

	    [NotNull]
	    public static Action<T> Action<T>()
	    {
		    return _ => { };
	    }
        [NotNull]
        public static Action<U, V> Action<U, V>()
        {
            return (u, v) => { };
        }
        public static Predicate<T> True<T>()
        {
            return _ => true;
        }
        public static Predicate<T> False<T>()
        {
            return _ => false;
        }
    }
}