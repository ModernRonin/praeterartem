using System;
using System.Collections.Generic;
using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using ModernRonin.PraeterArtem.Scheduling;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Scheduling
{
    public sealed class ThrottlingSchedulerTests
    {
        static IProcessor<int> CreateProcessor()
        {
            return Substitute.For<IProcessor<int>>();
        }
        static IProcessor<int> AddProcessor(IScheduler<int> underTest)
        {
            var processor = CreateProcessor();
            underTest.AddProcessor(processor);
            return processor;
        }
        [Fact]
        public void AddProcessor_Sets_Certain_Properties_On_Added_Processor()
        {
            var underTest = new ThrottlingScheduler<int>
                            {
                                Duration =
                                    TimeSpan
                                    .FromMinutes
                                    (33),
                                MaximumRequestCountPerDuration
                                    = 13
                            };
            var processor = CreateProcessor();
            underTest.AddProcessor(processor);
            processor.MaximumRequestCountPerDuration.Should()
                     .Be(underTest.MaximumRequestCountPerDuration);
            processor.Duration.Should().Be(underTest.Duration);
        }
        [Fact]
        public void NumberOfProcessors_Is_Decreased_With_RemoveProcessor()
        {
            var underTest = new ThrottlingScheduler<int>();
            var processors = new IProcessor<int>[5];
            5.TimesExecute(i =>
                           {
                               var processor = CreateProcessor();
                               processors[i] = processor;
                               underTest.AddProcessor(processor);
                           });
            5.TimesExecute(i =>
                           {
                               underTest.NumberOfProcessors.Should()
                                        .Be(5 - i);
                               underTest.RemoveProcessor(processors[i]);
                               underTest.NumberOfProcessors.Should()
                                        .Be(4 - i);
                           });
        }
        [Fact]
        public void NumberOfProcessors_Is_Increased_With_AddProcessor()
        {
            var underTest = new ThrottlingScheduler<int>();
            5.TimesExecute(i =>
                           {
                               underTest.NumberOfProcessors.Should().Be(i);
                               underTest.AddProcessor(CreateProcessor());
                               underTest.NumberOfProcessors.Should()
                                        .Be(1 + i);
                           });
        }
        [Fact]
        public void ProcessQueue_With_No_Processors_Increases_QueueSize()
        {
            var underTest = new ThrottlingScheduler<int>();
            underTest.AddRequest(Null.Action<int>());
            underTest.ProcessQueue();
            underTest.QueueSize.Should().Be(1);
        }
        [Fact]
        public void
            ProcessQueue_With_No_Requests_Does_Not_Call_Any_Processors()
        {
            var underTest = new ThrottlingScheduler<int>();
            var processor = AddProcessor(underTest);
            underTest.ProcessQueue();
            processor.DidNotReceiveWithAnyArgs().Execute(null);
        }
        [Fact]
        public void
            ProcessQueue_With_Single_Processor_And_Multiple_Requests_Executes_Requests_In_Fifo_Order
            ()
        {
            var requests = new Action<int>[] {_ => { }, _ => { }};
            var queue = new Queue<Action<int>>();
            Action<CallInfo> useRequest =
                c => queue.Enqueue(c.Arg<Action<int>>());

            var underTest = new ThrottlingScheduler<int>();
            var processor = AddProcessor(underTest);
            requests.UseIn(underTest.AddRequest);
            requests.UseIn(
                           r =>
                               processor.When(p => p.Execute(r))
                                        .Do(useRequest));

            processor.CanCurrentlyExecuteRequest.Returns(true);
            underTest.ProcessQueue();
            underTest.QueueSize.Should().Be(0);
            requests.UseIn(r => queue.Dequeue().Should().BeSameAs(r));
            queue.IsEmpty().Should().BeTrue();
        }
        [Fact]
        public void
            ProcessQueue_With_Single_Processor_And_Single_Request_Executes_Requests_And_Decreases_QueueSize
            ()
        {
            var underTest = new ThrottlingScheduler<int>();
            var processor = AddProcessor(underTest);
            var request = Null.Action<int>();
            underTest.AddRequest(request);
            processor.CanCurrentlyExecuteRequest.Returns(true);
            underTest.ProcessQueue();
            processor.Received().Execute(request);
            underTest.QueueSize.Should().Be(0);
        }
        [Fact]
        public void Properties_Of_Freshly_Created_Instance()
        {
            var underTest = new ThrottlingScheduler<int>();
            underTest.ShouldBeEquivalentTo(
                                           new
                                           {
                                               NumberOfProcessors = 0,
                                               QueueSize = 0,
                                               WeightOfLag = 1,
                                               WeightOfLoad = 1
                                           },
                cfg => cfg.ExcludingMissingProperties());
        }
        [Fact]
        public void QueueSize_Increases_With_AddRequest()
        {
            var underTest = new ThrottlingScheduler<int>();
            5.TimesExecute(i =>
                           {
                               underTest.QueueSize.Should().Be(i);
                               underTest.AddRequest(Null.Action<int>());
                               underTest.QueueSize.Should().Be(1 + i);
                           });
        }
    }
}