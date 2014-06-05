using System;

namespace ModernRonin.PraeterArtem.Scheduling
{
    /// <summary>
    /// Defines a processor which takes care of throttling of requests and
    /// accounts for average lag from initialization of a request to its
    /// fulfillment.
    /// </summary>
    /// <typeparam name="TWrappedProcessor"></typeparam>
    public interface IProcessor<out TWrappedProcessor> :
        IThrottlingConfigurable
    {
        /// <summary>The average time it took to execute a request.</summary>
        TimeSpan AverageLag { get; }
        /// <summary>
        /// Can a request currently be executed, given throttling parameters
        /// and latest load?
        /// </summary>
        bool CanCurrentlyExecuteRequest { get; }
        /// <summary>How many requests count towards
        /// <see cref="IThrottlingConfigurable.MaximumRequestCountPerDuration"/>
        /// currently?</summary>
        int CurrentRequestCountTowardsLimit { get; }
        /// <summary>Execute a request.</summary>
        /// <param name="request"></param>
        void Execute(Action<TWrappedProcessor> request);
    }
}