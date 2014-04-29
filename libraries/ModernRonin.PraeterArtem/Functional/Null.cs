using System;
using System.Collections.Generic;

namespace ModernRonin.PraeterArtem.Functional
{
    public static class Null
    {
		// Ilya: Delete this. There is Enumerable.Empty<T>()
		// which resharper finds if you press ctrl+alt+space
        public static IEnumerable<T> Enumerable<T>()
        {
			return System.Linq.Enumerable.Empty<T>();
        }
        public static Action Action()
        {
            return () => { };
        }
        public static Action<T> Action<T>()
        {
            return _ => { };
        }

    }
}