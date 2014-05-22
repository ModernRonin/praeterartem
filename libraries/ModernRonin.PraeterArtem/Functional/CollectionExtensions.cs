using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernRonin.PraeterArtem.Functional
{
    /// <summary>Contains extension methods for <see cref="ICollection{T}"/>.</summary>
    public static class CollectionExtensions
    {
        /// <summary>Removes all elements from <paramref name="collection"/> matching
        /// <paramref name="predicate"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        public static void RemoveWhere<T>(this ICollection<T> collection,
                                          Func<T, bool> predicate)
        {
            var matching = new List<T>();
            matching.AddRange(collection.Where(predicate));
            matching.UseIn(e => collection.Remove(e));
        }
    }
}