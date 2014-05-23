using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModernRonin.PraeterArtem.Functional;

namespace ModernRonin.PraeterArtem.Scheduling
{
    /// <summary>
    /// Implements <see cref="IProcessor{T}"/>.
    /// </summary>
    /// <typeparam name="TWrappedProcessor"></typeparam>
    public sealed class Processor<TWrappedProcessor> : AThrottlingConfigurable,
                                                IProcessor<TWrappedProcessor>
    {
        readonly Dictionary<DateTime, TimeSpan> mHistory =
            new Dictionary<DateTime, TimeSpan>();
        readonly object mLocker = new object();
        readonly ITimeGiver mTimeGiver;
        readonly TWrappedProcessor mWrappedProcessor;
        bool mIsExecutingRequest;
        public Processor(TWrappedProcessor wrappedProcessor,
                         ITimeGiver timeGiver)
        {
            mWrappedProcessor = wrappedProcessor;
            mTimeGiver = timeGiver;
        }
        public TimeSpan AverageLag
        {
            get
            {
                PruneHistory();
                if (mHistory.IsEmpty())
                    return TimeSpan.FromMilliseconds(0);
                return
                    TimeSpan.FromMilliseconds(
                                              mHistory.Values.Average(
                                                                      t =>
                                                                          t
                                                                              .TotalMilliseconds));
            }
        }
        public bool CanCurrentlyExecuteRequest
        {
            get
            {
                if (mIsExecutingRequest)
                    return false;
                return CurrentRequestCountTowardsLimit <
                       MaximumRequestCountPerDuration;
            }
        }
        public int CurrentRequestCountTowardsLimit
        {
            get
            {
                PruneHistory();
                return mHistory.Count;
            }
        }
        public void Execute(Action<TWrappedProcessor> request)
        {
            mIsExecutingRequest = true;
            // avoiding re-entrancy
            lock (mLocker)
            {
                var started = mTimeGiver.Now;
                mHistory.Add(started, TimeSpan.FromMilliseconds(0));
                var watch = new Stopwatch();
                watch.Start();
                request(mWrappedProcessor);
                watch.Stop();
                mHistory[started] = watch.Elapsed;
            }
            mIsExecutingRequest = false;
        }
        void PruneHistory()
        {
            var now = mTimeGiver.Now;
            var startOfThrottlingPeriod = now.Subtract(Duration);
            mHistory.RemoveWhere(kvp => kvp.Key < startOfThrottlingPeriod);
        }
    }
}