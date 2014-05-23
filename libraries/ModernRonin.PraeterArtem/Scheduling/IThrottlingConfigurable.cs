using System;

namespace ModernRonin.PraeterArtem.Scheduling
{
    /// <summary>
    /// Defines a contract for throttling of requests. For example, some
    /// service might require their users to not call them more often than 10 times
    /// per 1 minute. In that case, <see cref="Duration"/> would be set to 1 minute,
    /// and <see cref="MaximumRequestCountPerDuration"/> would be set to 10.
    /// </summary>
    public interface IThrottlingConfigurable
    {
        TimeSpan Duration { get; set; }
        int MaximumRequestCountPerDuration { get; set; }
    }
}