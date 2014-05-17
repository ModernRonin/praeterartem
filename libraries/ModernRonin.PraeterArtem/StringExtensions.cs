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
    }
}