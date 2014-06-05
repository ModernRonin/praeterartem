using System;
using System.Collections.Concurrent;
using System.Linq;
using MoreLinq;

namespace ModernRonin.PraeterArtem.Scheduling
{
    /// <summary>Implements <see cref="IScheduler{T}"/>.</summary>
    /// <typeparam name="TWrappedProcessor"></typeparam>
    public sealed class ThrottlingScheduler<TWrappedProcessor> :
        AThrottlingConfigurable, IThrottlingConfigurable,
        IScheduler<TWrappedProcessor>
    {
        readonly ConcurrentDictionary<IProcessor<TWrappedProcessor>, object>
            mProcessors =
                new ConcurrentDictionary
                    <IProcessor<TWrappedProcessor>, object>();
        readonly ConcurrentQueue<Action<TWrappedProcessor>> mRequests =
            new ConcurrentQueue<Action<TWrappedProcessor>>();
        public ThrottlingScheduler()
        {
            WeightOfLag = 1;
            WeightOfLoad = 1;
        }
        /// <summary>
        /// When choosing a processor, how much importance should the
        /// processor's average lag have?
        /// </summary>
        public int WeightOfLag { get; set; }
        /// <summary>
        /// When choosing a processor, how much importance should the
        /// processor's current number of requests towards the limit have?
        /// </summary>
        public int WeightOfLoad { get; set; }
        bool HaveQueuedRequests
        {
            get { return mRequests.Any(); }
        }
        bool IsAnyProcessorAvailable
        {
            get
            {
                return mProcessors.Keys.Any(p => p.CanCurrentlyExecuteRequest);
            }
        }
        public int QueueSize
        {
            get { return mRequests.Count; }
        }
        public int NumberOfProcessors
        {
            get { return mProcessors.Count; }
        }
        public void AddProcessor(IProcessor<TWrappedProcessor> processor)
        {
            processor.Duration = Duration;
            processor.MaximumRequestCountPerDuration =
                MaximumRequestCountPerDuration;
            mProcessors.TryAdd(processor, null);
        }
        public void RemoveProcessor(IProcessor<TWrappedProcessor> processor)
        {
            object notNeeded;
            mProcessors.TryRemove(processor, out notNeeded);
        }
        public void AddRequest(Action<TWrappedProcessor> request)
        {
            mRequests.Enqueue(request);
        }
        public void ProcessQueue()
        {
            while (IsAnyProcessorAvailable && HaveQueuedRequests)
            {
                Action<TWrappedProcessor> request;
                mRequests.TryDequeue(out request);
                var processor = GetBestProcessor();
                processor.Execute(request);
            }
        }
        IProcessor<TWrappedProcessor> GetBestProcessor()
        {
            return
                mProcessors.Keys.Where(p => p.CanCurrentlyExecuteRequest)
                           .MinBy(EvaluateProcessor);
        }
        object EvaluateProcessor(IProcessor<TWrappedProcessor> processor)
        {
            return processor.CurrentRequestCountTowardsLimit*WeightOfLoad +
                   processor.AverageLag.TotalSeconds*WeightOfLag;
        }
    }
}