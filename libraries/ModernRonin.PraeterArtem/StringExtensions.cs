using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ModernRonin.PraeterArtem.Functional;

namespace ModernRonin.PraeterArtem
{
    /// <summary>
    /// Contains extension methods for strings, basically stuff frequently
    /// needed and missing in the BCL.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>Returns <paramref name="what"/> repeated <paramref name="count"/>
        /// times.</summary>
        /// <param name="what"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Repeat(this string what, int count)
        {
            var result = new StringBuilder();
            count.TimesExecute(() => result.Append(what));
            return result.ToString();
        }
        /// <summary>Creates an MD5 hash of the string in hexadecimal format.</summary>
        public static string ToMd5Hash(this string what)
        {
            var asBytes =
                MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(what));
            var result = new StringBuilder(2*asBytes.Length);
            asBytes.UseIn(b => result.Append(b.ToString("x2")));
            return result.ToString();
        }
        /// <summary>Returns the string from including the character at
        /// <paramref name="inclusiveIndex"/>.</summary>
        /// <param name="source"></param>
        /// <param name="inclusiveIndex"></param>
        /// <returns></returns>
        public static string From(this string source, int inclusiveIndex)
        {
            return source.Substring(inclusiveIndex,
                source.Length - inclusiveIndex);
        }
        /// <summary>Returns the string from the start until excluding the character at
        /// <paramref name="exclusiveIndex"/>.</summary>
        /// <param name="source"></param>
        /// <param name="exclusiveIndex"></param>
        /// <returns></returns>
        public static string Until(this string source, int exclusiveIndex)
        {
            return source.Substring(0, exclusiveIndex);
        }
        /// <summary>Returns the string before the first occurrence of any of
        /// <paramref name="patterns"/>.</summary>
        /// <param name="source"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static string Before(this string source,
                                    params string[] patterns)
        {
            var indexOfWhat = StartIndexOfAny(source, patterns);
            return 0 <= indexOfWhat ? source.Until(indexOfWhat) : source;
        }
        /// <summary>Returns the index of the first occurrence of any of
        /// <paramref name="patterns"/>.</summary>
        /// <param name="source"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static int StartIndexOfAny(this string source,
                                          params string[] patterns)
        {
            var pattern = patterns.FirstOrDefault(source.Contains);
            return null == pattern ? -1 : IndexOf(source, pattern);
        }
        static int IndexOf(string source, string what)
        {
            return source.IndexOf(what, StringComparison.InvariantCulture);
        }
        /// <summary>Returns the index after the first occurrence of any of
        /// <paramref name="patterns"/>.</summary>
        /// <param name="source"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static int EndIndexOfAny(this string source,
                                        params string[] patterns)
        {
            var pattern = patterns.FirstOrDefault(source.Contains);
            return null == pattern
                ? -1
                : IndexOf(source, pattern) + pattern.Length;
        }
        /// <summary>Returns the string after the first occurrence of any of
        /// <paramref name="patterns"/>.</summary>
        /// <param name="source"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static string After(this string source,
                                   params string[] patterns)
        {
            var indexOfWhat = EndIndexOfAny(source, patterns);
            return 0 <= indexOfWhat ? source.From(indexOfWhat) : string.Empty;
        }
        /// <summary>
        /// Splits <paramref name="text"/> into lines. Both CR and LF count as
        /// line separators. Empty lines are discarded.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitIntoLines(this string text)
        {
            return text.Split(new[] {"\n", "\r"},
                StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// Interprets <paramref name="text"/> as concatenated lines and
        /// returns the zero-based index of the line containing what in
        /// <paramref name="text"/> originally is at character index
        /// <paramref name="index"/>. If the character at character index
        /// <paramref name="index"/>
        /// is a line separator, then the index of the line before that is returned. If
        /// <paramref name="lineSeparators"/> is not specified, then a sequence of
        /// "CRLF" is assumed to be the single line separator.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="lineSeparators"></param>
        /// <returns></returns>
        public static int LineNumberOfIndex(this string text, int index,
                                            params string[] lineSeparators)
        {
            var separators = lineSeparators.Any()
                ? lineSeparators
                : new[] {"\r\n"};
            var result =
                text.Until(index + 1)
                    .Split(separators, StringSplitOptions.None)
                    .Length - 1;
            if (separators.Select(s => s.Last()).Any(c => c == text[index]))
                --result;
            return result;
        }
    }
}