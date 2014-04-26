using System;

namespace ModernRonin.PraeterArtem.Functional
{
    /// <summary>
    ///     Extension methods on <see cref="IComparable{T}" />.
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        ///     Returns whether <paramref name="value" /> is in
        ///     [<paramref name="inclusiveLowerBound" />; <paramref name="inclusiveUpperBound" />]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="inclusiveLowerBound"></param>
        /// <param name="inclusiveUpperBound"></param>
        /// <returns></returns>
        public static bool IsInClosedInterval<T>(this T value,
                                                 T inclusiveLowerBound,
                                                 T inclusiveUpperBound)
            where T : IComparable<T>
        {
            return value.CompareTo(inclusiveLowerBound) >= 0 &&
                   value.CompareTo(inclusiveUpperBound) <= 0;
        }
        /// <summary>
        ///     Returns whether <paramref name="value" /> is in
        ///     [<paramref name="exclusiveLowerBound" />; <paramref name="exclusiveUpperBound" />]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="exclusiveLowerBound"></param>
        /// <param name="exclusiveUpperBound"></param>
        /// <returns></returns>
        public static bool IsInOpenInterval<T>(this T value,
                                               T exclusiveLowerBound,
                                               T exclusiveUpperBound)
            where T : IComparable<T>
        {
            return value.CompareTo(exclusiveLowerBound) > 0 &&
                   value.CompareTo(exclusiveUpperBound) < 0;
        }
    }
}