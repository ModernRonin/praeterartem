using System;

namespace ModernRonin.PraeterArtem.Scheduling
{
    /// <summary>
    /// Base class for implementors of <see cref="IThrottlingConfigurable"/>.
    /// </summary>
    public abstract class AThrottlingConfigurable
    {
        protected AThrottlingConfigurable()
        {
            Duration = new TimeSpan(0, 0, 1);
            MaximumRequestCountPerDuration = 1;
        }
        public TimeSpan Duration { get; set; }
        public int MaximumRequestCountPerDuration { get; set; }
    }
}