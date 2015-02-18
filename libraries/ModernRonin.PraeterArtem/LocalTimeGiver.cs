using System;

namespace ModernRonin.PraeterArtem
{
    /// <summary>Implemements <see cref="ITimeGiver"/> with Now.</summary>
    public sealed class LocalTimeGiver : ITimeGiver
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}