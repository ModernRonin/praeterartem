using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Functional
{
    // Ilya: It's very important to have VERY intuitive names here
    // otherwise noone is going to use it. I'd even said if there's 
    // no superb name for some stuff there's no way to add it here
    /// <summary>Contains extension methods for <see cref="IEnumerable{T}"/>
    /// supporting a functional coding style.</summary>
    public static class EnumerableExtensions
    {
        /// <summary>More readable replacement for !Any{T}.</summary>
        public static bool IsEmpty<T>(
            [NotNull] this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        // Ilya: I'd propose to call it "Foreach". The name that is really intuitive
        /// <summary>Passes each argument of <paramref name="enumerable"/> to
        /// <paramref name="action"/>.</summary>
        public static void UseIn<T>([NotNull] this IEnumerable<T> enumerable,
                                    [NotNull] Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");
            if (action == null)
                throw new ArgumentNullException("action");
            foreach (var element in enumerable)
                action(element);
        }
        /// <summary>
        /// Calls each action contained in <paramref name="actions"/> with the
        /// argument <paramref name="argument"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ExecuteOn<T>(
            [NotNull] this IEnumerable<Action<T>> actions, T argument)
        {
            if (actions == null)
                throw new ArgumentNullException("actions");
            actions.UseIn(a => a(argument));
        }
        /// <summary>
        /// Allows to easily pass single elements into functions expecting an
        /// IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> ToEnumerable<T>(this T element)
        {
            if (element is IEnumerable<T>)
                return CastToEnumerable(element);
            return WrapAsEnumerable(element);
        }
        [NotNull]
        static IEnumerable<T> CastToEnumerable<T>(T singleElement)
        {
            return (IEnumerable<T>) singleElement;
        }
        [NotNull]
        static IEnumerable<T> WrapAsEnumerable<T>(T singleElement)
        {
            yield return singleElement;
        }
        /// <summary>Adds all element to <paramref name="destination"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="destination"></param>
        public static void AddTo<T>([NotNull] this IEnumerable<T> enumerable,
                                    [NotNull] ICollection<T> destination)
        {
            enumerable.UseIn(destination.Add);
        }

        // Ilya:
        // I'm not sure that "blah.ExceptNullValues()" 
        // is better then    "blah.Where(e => e != null)"
        // not much shorter either.
        /// <summary>Returns all elements which are not null.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> ExceptNullValues<T>(
            [NotNull] this IEnumerable<T> enumerable) where T : class
        {
            return enumerable.Where(e => null != e);
        }

        // Ilya: in general I'm afraid that seeing 
        // "items.ButFirst()" would freak me out
        // why not "items.Skip(1)" I would think?!
        /// <summary>Returns all elements except the first.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> ButFirst<T>(
            [NotNull] this IEnumerable<T> enumerable)
        {
            return enumerable.Skip(1);
        }
        /// <summary>Returns all elements except the last.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> ButLast<T>(
            [NotNull] this IEnumerable<T> enumerable)
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
        /// <summary>Returns all elements of <paramref name="enumerable"/> except
        /// <paramref name="value"/>/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> Except<T>(
            [NotNull] this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Except(value.ToEnumerable());
        }
        /// <summary>Returns all elements of <paramref name="enumerable"/>
        /// which are greater than <paramref name="limit"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> GreaterThan<T>(
            [NotNull] this IEnumerable<T> enumerable, T limit)
            where T : IComparable<T>
        {
            return enumerable.Where(e => 0 < e.CompareTo(limit));
        }
        /// <summary>Returns all elements of <paramref name="enumerable"/>
        /// which are greater than or equal to <paramref name="limit"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> GreaterThanOrEqualTo<T>(
            [NotNull] this IEnumerable<T> enumerable, T limit)
            where T : IComparable<T>
        {
            return enumerable.Where(e => 0 <= e.CompareTo(limit));
        }
        /// <summary>Returns all elements of <paramref name="enumerable"/>
        /// which are smaller than <paramref name="limit"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> SmallerThan<T>(
            [NotNull] this IEnumerable<T> enumerable, T limit)
            where T : IComparable<T>
        {
            return enumerable.Where(e => 0 > e.CompareTo(limit));
        }
        /// <summary>Returns all elements of <paramref name="enumerable"/>
        /// which are smaller than or equal to<paramref name="limit"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> SmallerThanOrEqualTo<T>(
            [NotNull] this IEnumerable<T> enumerable, T limit)
            where T : IComparable<T>
        {
            return enumerable.Where(e => 0 >= e.CompareTo(limit));
        }
        /// <summary>
        /// Returns all elements of <paramref name="enumerable"/> which are
        /// equal to <paramref name="check"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> EqualTo<T>(
            [NotNull] this IEnumerable<T> enumerable, T check)
        {
            return enumerable.Where(e => e.Equals(check));
        }
        /// <summary>Returns all elements of <paramref name="enumerable"/>
        /// which are reference-equal to <paramref name="check"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> SameAs<T>(
            [NotNull] this IEnumerable<T> enumerable, T check)
        {
            return enumerable.Where(e => ReferenceEquals(e, check));
        }
        /// <summary>
        /// Returns the element of <paramref name="enumerable"/>
        /// with the smallest evaluation when passed to <paramref name="evaluator"/>.
        /// (In contrast to the BCL .Min() method which returns the minimal value, not
        /// the minimal element.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static T MinElement<T>(
            [NotNull] this IEnumerable<T> enumerable,
            [NotNull] Func<T, int> evaluator)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");
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
        /// Returns the element of <paramref name="enumerable"/>
        /// with the greatest evaluation when passed to <paramref name="evaluator"/>.
        /// (In contrast to the BCL .Max() method which returns the minimal value, not
        /// the minimal element.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static T MaxElement<T>(
            [NotNull] this IEnumerable<T> enumerable,
            [NotNull] Func<T, int> evaluator)
        {
            return enumerable.MinElement(e => -evaluator(e));
        }

        // Ilya: elements.Min(5) is a bit obscure.
        // May be elements.MinOrDefault(5)?
        /// <summary>
        /// Overload for the BCL .Min() method which allows to pass a value to
        /// be used in case
        /// <paramref name="enumerable"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueToReturnIfEmpty"></param>
        /// <returns></returns>
        public static T Min<T>([NotNull] this IEnumerable<T> enumerable,
                               T valueToReturnIfEmpty)
            where T : IComparable<T>
        {
            var frozen = enumerable as T[] ?? enumerable.ToArray();
            return frozen.Any() ? frozen.Min() : valueToReturnIfEmpty;
        }
        /// <summary>
        /// Overload for the BCL .Max() method which allows to pass a value to
        /// be used in case
        /// <paramref name="enumerable"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueToReturnIfEmpty"></param>
        /// <returns></returns>
        public static T Max<T>([NotNull] this IEnumerable<T> enumerable,
                               T valueToReturnIfEmpty)
            where T : IComparable<T>
        {
            var frozen = enumerable as T[] ?? enumerable.ToArray();
            return frozen.Any() ? frozen.Max() : valueToReturnIfEmpty;
        }
        /// <summary>
        /// Splits the enumerable into multiple enumerables of size
        /// <paramref name="chunkSize"/>. Note that the last chunk may contain fewer
        /// items.
        /// <example>
        /// new []{1,2,3,4,5,6,7,8,9, 0}.Chunk(3) will result in 3 chunks with
        /// 3 elements and a fourth chunk with only one element
        /// </example>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(
            this IEnumerable<T> enumerable, int chunkSize)
        {
            for (var rest = enumerable.ToArray();
                 !rest.IsEmpty();
                 rest = rest.Skip(chunkSize).ToArray())
                yield return rest.Take(chunkSize);
        }
        /// <summary>Returns an enumeration of all elements .ToString().</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings<T>(
            this IEnumerable<T> enumerable)
        {
            return enumerable.Select(e => e.ToString());
        }
    }
}