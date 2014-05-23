using System;

namespace ModernRonin.PraeterArtem.Scheduling
{
    /// <summary>Encapsulates scheduling of jobs over a multitude of
    /// <see cref="IProcessor{T}"/>s.</summary>
    /// <typeparam name="TWrappedProcessor"></typeparam>
    public interface IScheduler<TWrappedProcessor>
    {
        /// <summary>The number of queued requestes.</summary>
        int QueueSize { get; }
        /// <summary>The number of processors registered with this scheduler.</summary>
        int NumberOfProcessors { get; }
        /// <summary>Add a processor to this scheduler.</summary>
        /// <param name="processor"></param>
        void AddProcessor(IProcessor<TWrappedProcessor> processor);
        /// <summary>Remove a processor from this scheduler.</summary>
        /// <param name="processor"></param>
        void RemoveProcessor(IProcessor<TWrappedProcessor> processor);
        /// <summary>Add a request to the queue of this scheduler.</summary>
        /// <param name="request"></param>
        void AddRequest(Action<TWrappedProcessor> request);
        /// <summary>
        /// Process all requests currently queued. This MUST be called
        /// regularly, otherwise no requests are processed at all.
        /// </summary>
        void ProcessQueue();
    }
}