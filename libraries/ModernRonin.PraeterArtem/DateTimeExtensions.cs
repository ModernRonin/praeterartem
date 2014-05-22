using System;

namespace ModernRonin.PraeterArtem
{
    /// <summary>
    /// Contains extension methods for DateTime, basically stuff frequently
    /// needed and missing in the BCL.
    /// </summary>
    public static class DateTimeExtensions
    {
        static DateTime sUnixBeginningOfTime = new DateTime(1970, 1, 1, 0, 0,
            0, DateTimeKind.Utc);
        /// <summary>
        /// Returns a unix-timestamp for the given DateTime. DateTime can be
        /// local or UTC.
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static int ToUnixTime(this DateTime rhs)
        {
            if (rhs.Kind != DateTimeKind.Utc)
                rhs = rhs.ToUniversalTime();
            return (int) rhs.Subtract(sUnixBeginningOfTime).TotalSeconds;
        }
        /// <summary>Returns a UTC datetime for a given unix-timestamp.</summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeToUtc(int rhs)
        {
            return sUnixBeginningOfTime.AddSeconds(rhs);
        }
    }
}