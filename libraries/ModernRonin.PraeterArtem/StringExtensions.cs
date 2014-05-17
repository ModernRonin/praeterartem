using System;
using System.Security.Cryptography;
using System.Text;
using ModernRonin.PraeterArtem.Functional;

namespace ModernRonin.PraeterArtem
{
    /// <summary>
    ///     Contains extension methods for strings, basically stuff frequently needed and missing in the BCL.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Returns <paramref name="what" /> repeated <paramref name="count" /> times.
        /// </summary>
        /// <param name="what"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Repeat(this string what, int count)
        {
            var result = new StringBuilder();
            count.TimesExecute(() => result.Append(what));
            return result.ToString();
        }
        /// <summary>
        /// Creates an MD5 hash of the string in hexadecimal format. 
        /// </summary>
        public static string ToMd5Hash(this string what)
        {
            var asBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(what));
            var result = new StringBuilder(2 * asBytes.Length);
            asBytes.UseIn(b => result.Append(b.ToString("x2")));
            return result.ToString();
        }
        public static string From(this string source, int inclusiveIndex)
        {
            return source.Substring(inclusiveIndex, source.Length - inclusiveIndex);
        }

        public static string Until(this string source, int exclusiveIndex)
        {
            return source.Substring(0, exclusiveIndex);
        }

        public static string Before(this string source, params string[] patterns)
        {
            var indexOfWhat = StartIndexOfAny(source, patterns);
            return 0 <= indexOfWhat ? source.Until(indexOfWhat) : source;
        }

        public static int StartIndexOfAny(this string source, params string[] patterns)
        {
            foreach (var what in patterns)
            {
                var indexOfWhat = source.IndexOf(what, StringComparison.InvariantCulture);
                if (indexOfWhat >= 0) return indexOfWhat;
            }
            return -1;
        }

        public static int EndIndexOfAny(this string source, params string[] patterns)
        {
            foreach (var what in patterns)
            {
                var indexOfWhat = source.IndexOf(what, StringComparison.InvariantCulture);
                if (indexOfWhat >= 0) return indexOfWhat + what.Length;
            }
            return -1;
        }

        public static string After(this string source, params string[] patterns)
        {
            var indexOfWhat = EndIndexOfAny(source, patterns);
            return 0 <= indexOfWhat ? source.From(indexOfWhat) : string.Empty;
        }
    }
}