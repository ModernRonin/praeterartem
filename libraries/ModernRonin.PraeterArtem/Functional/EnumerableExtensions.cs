using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernRonin.PraeterArtem.Functional
{
    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" /> supporting a
    ///     functional coding style.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     More readable replacement for !Any{T}.
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
        /// <summary>
        ///     Passes each argument of <paramref name="enumerable" /> to <paramref name="action" />.
        /// </summary>
        public static void UseIn<T>(this IEnumerable<T> enumerable,
                                    Action<T> action)
        {
            foreach (var element in enumerable)
                action(element);
        }
        /// <summary>
        ///     Calls each action contained in <paramref name="actions" /> with the argument <paramref name="argument" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ExecuteOn<T>(this IEnumerable<Action<T>> actions,
                                        T argument)
        {
            actions.UseIn(a => a(argument));
        }
        /// <summary>
        ///     Allows to easily pass single elements into functions expecting
        ///     an IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this T element)
        {
            if (element is IEnumerable<T>)
                return CastToEnumerable(element);
            return WrapAsEnumerable(element);
        }
        static IEnumerable<T> CastToEnumerable<T>(T singleElement)
        {
            return singleElement as IEnumerable<T>;
        }
        static IEnumerable<T> WrapAsEnumerable<T>(T singleElement)
        {
            yield return singleElement;
        }
        /// <summary>
        ///     Adds all element to <paramref name="destination" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="destination"></param>
        public static void AddTo<T>(this IEnumerable<T> enumerable,
                                    ICollection<T> destination)
        {
            enumerable.UseIn(destination.Add);
        }
        /// <summary>
        ///     Returns all elements which are not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExceptNullValues<T>(
            this IEnumerable<T> enumerable) where T : class
        {
            return enumerable.Where(e => null != e);
        }
        /// <summary>
        ///     Returns all elements except the first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> ButFirst<T>(
            this IEnumerable<T> enumerable)
        {
            return enumerable.Skip(1);
        }
        /// <summary>
        ///     Returns all elements except the last.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> ButLast<T>(
            this IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
                yield break;
            var first = enumerator.Current;
            while (enumerator.MoveNext())
            {
                yield return first;
                first = enumerator.Current;
            }
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" /> except <paramref name="value" />/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T>(
            this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Except(value.ToEnumerable());
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" />
        ///     which are greater than <paramref name="limit" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<T> GreaterThan<T>(
            this IEnumerable<T> enumerable, T limit) where T : IComparable<T>
        {
            return enumerable.Where(e => 0 < e.CompareTo(limit));
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" />
        ///     which are greater than or equal to <paramref name="limit" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<T> GreaterThanOrEqualTo<T>(
            this IEnumerable<T> enumerable, T limit) where T : IComparable<T>
        {
            return enumerable.Where(e => 0 <= e.CompareTo(limit));
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" />
        ///     which are smaller than <paramref name="limit" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<T> SmallerThan<T>(
            this IEnumerable<T> enumerable, T limit) where T : IComparable<T>
        {
            return enumerable.Where(e => 0 > e.CompareTo(limit));
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" />
        ///     which are smaller than or equal to<paramref name="limit" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<T> SmallerThanOrEqualTo<T>(
            this IEnumerable<T> enumerable, T limit) where T : IComparable<T>
        {
            return enumerable.Where(e => 0 >= e.CompareTo(limit));
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" /> which are equal to <paramref name="check" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static IEnumerable<T> EqualTo<T>(
            this IEnumerable<T> enumerable, T check)
        {
            return enumerable.Where(e => e.Equals(check));
        }
        /// <summary>
        ///     Returns all elements of <paramref name="enumerable" />
        ///     which are reference-equal to <paramref name="check" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static IEnumerable<T> SameAs<T>(
            this IEnumerable<T> enumerable, T check)
        {
            return enumerable.Where(e => ReferenceEquals(e, check));
        }
        /// <summary>
        ///     Returns the element of <paramref name="enumerable" />
        ///     with the smallest evaluation when passed to <paramref name="evaluator" />.
        ///     (In contrast to the BCL .Min() method which returns the minimal value, not the
        ///     minimal element.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static T MinElement<T>(this IEnumerable<T> enumerable,
                                      Func<T, int> evaluator)
        {
            var minimumScore = Int32.MaxValue;
            var result = default(T);
            foreach (var element in enumerable)
            {
                var score = evaluator(element);
                if (score <= minimumScore)
                {
                    minimumScore = score;
                    result = element;
                }
            }
            return result;
        }
        /// <summary>
        ///     Returns the element of <paramref name="enumerable" />
        ///     with the greatest evaluation when passed to <paramref name="evaluator" />.
        ///     (In contrast to the BCL .Max() method which returns the minimal value, not the
        ///     minimal element.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static T MaxElement<T>(this IEnumerable<T> enumerable,
                                      Func<T, int> evaluator)
        {
            return enumerable.MinElement(e => -evaluator(e));
        }
        /// <summary>
        ///     Overload for the BCL .Min() method
        ///     which allows to pass a value to be used in case
        ///     <paramref name="enumerable" /> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueToReturnIfEmpty"></param>
        /// <returns></returns>
        public static T Min<T>(this IEnumerable<T> enumerable,
                               T valueToReturnIfEmpty)
            where T : IComparable<T>
        {
            var frozen = enumerable as T[] ?? enumerable.ToArray();
            return frozen.Any() ? frozen.Min() : valueToReturnIfEmpty;
        }
        /// <summary>
        ///     Overload for the BCL .Max() method
        ///     which allows to pass a value to be used in case
        ///     <paramref name="enumerable" /> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueToReturnIfEmpty"></param>
        /// <returns></returns>
        public static T Max<T>(this IEnumerable<T> enumerable,
                               T valueToReturnIfEmpty)
            where T : IComparable<T>
        {
            var frozen = enumerable as T[] ?? enumerable.ToArray();
            return frozen.Any() ? frozen.Max() : valueToReturnIfEmpty;
        }
    }
}