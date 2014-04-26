using System;
using System.Collections.Generic;

namespace ModernRonin.PraeterArtem.Functional
{
    public static class Null
    {
        public static IEnumerable<T> Enumerable<T>()
        {
            yield break;
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