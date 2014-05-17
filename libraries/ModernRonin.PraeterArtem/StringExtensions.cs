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
        public static string ToMd5Hash(this string rhs)
        {
            var asBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(rhs));
            var result = new StringBuilder(2 * asBytes.Length);
            asBytes.UseIn(b => result.Append(b.ToString("x2")));
            return result.ToString();
        }
    }
}