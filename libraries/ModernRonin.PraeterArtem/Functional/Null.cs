using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Functional
{
    public static class Null
    {
		// Ilya: Delete this. There is Enumerable.Empty<T>()
		// which resharper finds if you press Control+Alt+Space
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
    }
}