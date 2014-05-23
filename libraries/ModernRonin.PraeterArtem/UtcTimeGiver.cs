using System;

namespace ModernRonin.PraeterArtem
{
    /// <summary>Implemements <see cref="ITimeGiver"/> with UtcNow.</summary>
    public sealed class UtcTimeGiver : ITimeGiver
    {
        public DateTime Now
        {
            get { return DateTime.UtcNow; }
        }
    }
}