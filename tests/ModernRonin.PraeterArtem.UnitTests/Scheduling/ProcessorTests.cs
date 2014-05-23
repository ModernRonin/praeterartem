using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using ModernRonin.PraeterArtem.Scheduling;
using NSubstitute;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Scheduling
{
    public sealed class ProcessorTests
    {
        readonly ITimeGiver mTimeGiver;
        readonly Processor<int> mUnderTest;
        public ProcessorTests()
        {
            mTimeGiver = Substitute.For<ITimeGiver>();
            mUnderTest = new Processor<int>(13, mTimeGiver);
        }
        void LetTimeGiverUseRealTime()
        {
            mTimeGiver.Now.Returns(_ => DateTime.UtcNow);
        }
        [Fact]
        public void
            CanCurrentlyExecuteRequest_Is_False_If_RequestCount_Exceeds_Maximum_Within_Duration
            ()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.Duration = TimeSpan.FromHours(2);
            mUnderTest.MaximumRequestCountPerDuration = 5;
            Action<int> request = _ => Thread.Sleep(100);
            4.TimesExecute(i =>
                           {
                               mUnderTest.CurrentRequestCountTowardsLimit
                                         .Should().Be(i);
                               mUnderTest.Execute(request);
                               mUnderTest.CanCurrentlyExecuteRequest.Should()
                                         .BeTrue();
                               mUnderTest.CurrentRequestCountTowardsLimit
                                         .Should().Be(1 + i);
                           });
            mUnderTest.Execute(request);
            mUnderTest.CanCurrentlyExecuteRequest.Should().BeFalse();
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(5);
        }
        [Fact]
        public void
            CanCurrentlyExecuteRequest_Is_True_After_Completion_Of_Request_If_No_Limit_Hit
            ()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.MaximumRequestCountPerDuration = int.MaxValue;
            mUnderTest.Execute(Null.Action<int>());
            mUnderTest.CanCurrentlyExecuteRequest.Should().BeTrue();
        }
        [Fact]
        public void
            CanCurrentlyExecuteRequest_Is_True_Again_After_Duration_Runs_Over
            ()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.Duration = TimeSpan.FromHours(2);
            mUnderTest.MaximumRequestCountPerDuration = 2;
            Action<int> request = _ => Thread.Sleep(100);
            var future = DateTime.UtcNow.Add(TimeSpan.FromMinutes(121));
            mUnderTest.Execute(request);
            mUnderTest.Execute(request);
            mUnderTest.CanCurrentlyExecuteRequest.Should().BeFalse();
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(2);
            mTimeGiver.Now.Returns(future);
            mUnderTest.CanCurrentlyExecuteRequest.Should().BeTrue();
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(0);
        }
        [Fact]
        public void CurrentRequestCountTowardsLimit_Falls_Off_Over_Time()
        {
            mUnderTest.Duration = TimeSpan.FromSeconds(1);
            mUnderTest.MaximumRequestCountPerDuration = 3;
            Action<int> request = _ => Thread.Sleep(100);
            var start = DateTime.UtcNow;

            var timesFromNow =
                new[] {500, 750, 950, 1000, 1600, 1750, 1900}.Select(
                                                                     ToTimeSpan);
            var timesToReturn = timesFromNow.Select(start.Add);

            mTimeGiver.Now.Returns(start, timesToReturn.ToArray());
            mUnderTest.Execute(request);
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(1);
            mUnderTest.Execute(request);
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(2);
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(2);
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(1);
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(1);
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(0);
        }
        static TimeSpan ToTimeSpan(int ms)
        {
            return TimeSpan.FromMilliseconds(ms);
        }
        [Fact]
        public void Execute_Should_Really_Be_Executed()
        {
            LetTimeGiverUseRealTime();
            var gotten = 0;
            mUnderTest.Execute(p => gotten = p);
            gotten.Should().Be(13);
        }
        [Fact]
        public void Multiple_Calls_To_Execute_Set_Correct_Statistics()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.Execute(_ => Thread.Sleep(500));
            mUnderTest.Execute(_ => Thread.Sleep(300));
            mUnderTest.Execute(_ => Thread.Sleep(100));
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(3);
            mUnderTest.AverageLag.TotalMilliseconds.Should()
                      .BeInRange(250d, 350d);
        }
        [Fact]
        public void Overlapping_Calls_To_Execute_Should_Be_Sequentialized()
        {
            LetTimeGiverUseRealTime();
            var count = 0;
            Action<int> request1 = i =>
                                   {
                                       Thread.Sleep(750);
                                       count += 2;
                                   };
            Action<int> request2 = i =>
                                   {
                                       Thread.Sleep(500);
                                       count += 4;
                                   };
            var firstCall = new Task(() => mUnderTest.Execute(request1));
            var secondCall = new Task(() => mUnderTest.Execute(request2));
            firstCall.Start();
            count.Should().Be(0);
            secondCall.Start();
            count.Should().Be(0);
            firstCall.IsCompleted.Should().BeFalse();
            secondCall.IsCompleted.Should().BeFalse();
            firstCall.Wait();
            secondCall.IsCompleted.Should().BeFalse();
            count.Should().Be(2);
            secondCall.Wait();
            count.Should().Be(6);
        }
        [Fact]
        public void Properties_Of_Freshly_Constructed_Instance()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.ShouldBeEquivalentTo(
                                            new
                                            {
                                                AverageLag =
                                                    TimeSpan
                                                        .FromMilliseconds
                                                        (0),
                                                CanCurrentlyExecuteRequest
                                                    = true,
                                                CurrentRequestCountTowardsLimit
                                                    = 0
                                            },
                cfg => cfg.ExcludingMissingProperties());
        }
        [Fact]
        public void Single_Call_To_Execute_Sets_Correct_Statistics()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.Execute(_ => Thread.Sleep(500));
            mUnderTest.CurrentRequestCountTowardsLimit.Should().Be(1);
            mUnderTest.AverageLag.TotalMilliseconds.Should()
                      .BeInRange(450d, 550d);
        }
        [Fact]
        public void
            While_Execute_Is_Running_CanCurrentlyExecuteRequest_Should_Be_False
            ()
        {
            LetTimeGiverUseRealTime();
            mUnderTest.MaximumRequestCountPerDuration = int.MaxValue;
            var waiter = new AutoResetEvent(false);
            var call = new Task(() => mUnderTest.Execute(_ =>
                                                         {
                                                             waiter.Set();
                                                             Thread.Sleep(
                                                                          1000);
                                                         }));
            call.Start();
            waiter.WaitOne();
            mUnderTest.CanCurrentlyExecuteRequest.Should().BeFalse();
            call.Wait();
            mUnderTest.CanCurrentlyExecuteRequest.Should().BeTrue();
        }
    }
}